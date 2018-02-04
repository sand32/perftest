var express = require("express")
    _ = require("lodash"),
    app = express(),
    modelData = require("./data"),
    modelNextId = 9,
    modelSchema = {_id:0,name:"",job:"",birthDate:""},

    currentDomain = function(req){
        return req.protocol + "://" + req.get("host");
    },
    
    removeInternalProperties = function(data){
        return _.omitBy(data, (value, key) => {
            return _.startsWith(key, "_");
        });
    },

    search = function(req, data){
        req.search = _.omit(req.query, ["offset", "size", "fields", "sort"]);
        if(_.isEmpty(req.search)) return data;
        return _.filter(data, item => {
            return _.isMatch(item, req.search);
        });
    },

    sort = function(req, data){
        if(_.isEmpty(req.query.sort)) return data;
        var sortTerms = _.split(req.query.sort, ",");
        var directions = _.map(sortTerms, item => {return item[0] == "-" ? "desc" : "asc";});
        sortTerms = _.map(sortTerms, item => {return item[0] == "-" ? item.substring(1) : item;});
        return _.orderBy(data, sortTerms, directions);
    },

    paginate = function(req, data, maxPageSize){
        var offset = 0,
            size = maxPageSize;

        if(!_.isEmpty(req.query.offset) && typeof _.parseInt(req.query.offset) === Number){
            offset = _.parseInt(req.query.offset);
        }else{
            offset = 0;
        }

        if(!_.isEmpty(req.query.size) && typeof _.parseInt(req.query.size) === Number){
            size = Math.min(_.parseInt(req.query.size), maxPageSize);
        }else{
            size = maxPageSize;
        }

        return _.slice(data, offset, offset + size);
    },
    
    formatDataForSelection = function(req, data, collectionPath, resourceFields){
        if(_.isEmpty(req.query.fields)){
            return _.map(data, item => {
                let id = item._id;
                item = removeInternalProperties(item);
                item._href = currentDomain(req) + collectionPath + "/" + id
                return item;
            });
        }else{
            let fields = _.split(req.query.fields, ",").map(_.trim);
            fields = _.intersection(fields, _.keys(resourceFields));
            return _.map(data, item => {
                let id = item._id;
                item = _.omitBy(item, (value, key) => {
                    return _.startsWith(key, "_") || !_.includes(fields, key);
                });
                item._href = currentDomain(req) + collectionPath + "/" + id
                return item;
            });
        }
    };

app.get("/", (req, res) => {
    var filteredData = modelData;
    filteredData = search(req, filteredData);
    filteredData = sort(req, filteredData);
    filteredData = paginate(req, filteredData, 30);
    filteredData = formatDataForSelection(req, filteredData, "", modelSchema);
    return res.send(filteredData);
});

app.get("/:id", (req, res) => {
    var element = _.find(modelData, item => item._id == req.params.id);
    if(!element) return res.status(404).end();
    res.send(element);
});

app.post("/", (req, res) => {
    if(_.difference(_.keys(req.body), _.keys(modelSchema)) !== []) return res.status(400).end();
    req.body._id = modelNextId++;
    modelData.push(req.body);
    res.set("Location", currentDomain(req) + "/" + req.body._id);
    return res.status(201).end();
});

app.put("/:id", (req, res) => {
    var index = _.indexOf(modelData, item => item._id == req.params.id);
    if(index === -1) return res.status(404).end();
    req.body._id = element._id
    modelData[index] = req.body;
    return res.send(req.body);
});

app.delete("/:id", (req, res) => {
    var removed = _.remove(modelData, item => item._id == req.params.id);
    if(removed.length === 0) return res.status(404).end();
    return res.status(204).end();
});

app.listen(5000);

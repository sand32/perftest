using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace aspnetcore
{
    public class Datum
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public static class Data
    {
        public static int NextId { get; set; } = 9;
        public static List<Datum> Collection => new List<Datum>
        {
            new Datum
            {
                Id = 1,
                Name = "Jim-Bob-Joe",
                Job = "Mechanic",
                BirthDate = DateTime.UtcNow
            },
            new Datum
            {
                Id = 2,
                Name = "Mary Sue",
                Job = "Mechanic",
                BirthDate = DateTime.UtcNow
            },
            new Datum
            {
                Id = 3,
                Name = "Hubert Cumberdale",
                Job = "Friend",
                BirthDate = DateTime.UtcNow
            },
            new Datum
            {
                Id = 4,
                Name = "Margery Stewart-Baxter",
                Job = "Friend",
                BirthDate = DateTime.UtcNow
            },
            new Datum
            {
                Id = 5,
                Name = "Jeremy Fisher",
                Job = "Friend",
                BirthDate = DateTime.UtcNow
            },
            new Datum
            {
                Id = 6,
                Name = "Winnie-the-Pooh",
                Job = "Pooh Bear",
                BirthDate = DateTime.UtcNow
            },
            new Datum
            {
                Id = 7,
                Name = "Dora-the-Explorer",
                Job = "Explorer",
                BirthDate = DateTime.UtcNow
            },
            new Datum
            {
                Id = 8,
                Name = "Walter Cronkite",
                Job = "News Anchor",
                BirthDate = DateTime.UtcNow
            }
        };
    }
}

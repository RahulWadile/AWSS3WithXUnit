using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AWSDemo.Models
{
    public class Catalog
    {
        [JsonPropertyName("id")]

        public int Id { get; set; }

        [JsonPropertyName("name")]

        public string Name { get; set; }

        [JsonPropertyName("createAt")]

        public DateTime CreateAt { get; set; }

        [JsonPropertyName("modifiedAt")]

        public DateTime ModifiedAt { get; set; }


        [JsonPropertyName("uploadFilename")]

        public string uploadFilename { get; set; }

    }
}

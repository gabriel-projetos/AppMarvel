using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Thumbnail
    {
        public string path { get; private set; }
        public string extension { get; private set; }
    }

    public class Item
    {
        public string resourceURI { get; private set; }
        public string name { get; private set; }
    }

    public class Comics
    {
        public int available { get; private set; }
        public string collectionURI { get; private set; }
        public List<Item> items { get; private set; }
        public int returned { get; private set; }
    }

    public class Item2
    {
        public string resourceURI { get; private set; }
        public string name { get; private set; }
    }

    public class Series
    {
        public int available { get; private set; }
        public string collectionURI { get; private set; }
        public List<Item2> items { get; private set; }
        public int returned { get; private set; }
    }

    public class Item3
    {
        public string resourceURI { get; private set; }
        public string name { get; private set; }
        public string type { get; private set; }
    }

    public class Stories
    {
        public int available { get; private set; }
        public string collectionURI { get; private set; }
        public List<Item3> items { get; private set; }
        public int returned { get; private set; }
    }

    public class Item4
    {
        public string resourceURI { get; private set; }
        public string name { get; private set; }
    }

    public class Events
    {
        public int available { get; private set; }
        public string collectionURI { get; private set; }
        public List<Item4> items { get; private set; }
        public int returned { get; private set; }
    }

    public class Url
    {
        public string type { get; private set; }
        public string url { get; private set; }
    }

    public class Result
    {
        //[JsonProperty()]
        public int id { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }

        //[Jil.JilDirective(true), JsonIgnore]
        public DateTime modified { get; private set; }

        //[Jil.JilDirective(true), JsonIgnore]
        public Thumbnail thumbnail { get; private set; }
        public string resourceURI { get; private set; }
        public Comics comics { get; private set; }
        public Series series { get; private set; }
        public Stories stories { get; private set; }
        public Events events { get; private set; }
        public List<Url> urls { get; private set; }

        public string GetImage
        {
            get { return $"{thumbnail.path}.{thumbnail.extension}"; }
        }

    }

    public class Data
    {
        public int offset { get; private set; }
        public int limit { get; private set; }
        public int total { get; private set; }
        public int count { get; private set; }
        public IEnumerable<Result> results { get; private set; }
    }

    public class Hero
    {
        public int code { get; private set; }
        public string status { get; private set; }
        public string copyright { get; private set; }
        public string attributionText { get; private set; }
        public string attributionHTML { get; private set; }
        public string etag { get; private set; }
        public Data data { get; private set; }
    }




}

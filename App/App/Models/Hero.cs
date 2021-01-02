using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Thumbnail
    {
        public string path { get; set; }
        public string extension { get; set; }
    }

    public class Item
    {
        public string resourceURI { get; set; }
        public string name { get; set; }
    }

    public class Comics
    {
        public int available { get; set; }
        public string collectionURI { get; set; }
        public List<Item> items { get; set; }
        public int returned { get; set; }
    }

    public class Item2
    {
        public string resourceURI { get; set; }
        public string name { get; set; }
    }

    public class Series
    {
        public int available { get; set; }
        public string collectionURI { get; set; }
        public List<Item2> items { get; set; }
        public int returned { get; set; }
    }

    public class Item3
    {
        public string resourceURI { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Stories
    {
        public int available { get; set; }
        public string collectionURI { get; set; }
        public List<Item3> items { get; set; }
        public int returned { get; set; }
    }

    public class Item4
    {
        public string resourceURI { get; set; }
        public string name { get; set; }
    }

    public class Events
    {
        public int available { get; set; }
        public string collectionURI { get; set; }
        public List<Item4> items { get; set; }
        public int returned { get; set; }
    }

    public class Url
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime modified { get; set; }
        public Thumbnail thumbnail { get; set; }
        public string resourceURI { get; set; }
        public Comics comics { get; set; }
        public Series series { get; set; }
        public Stories stories { get; set; }
        public Events events { get; set; }
        public List<Url> urls { get; set; }

        public string GetImage
        {
            get { return $"{thumbnail.path}.{thumbnail.extension}"; }
        }

    }

    public class Data
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public int count { get; set; }
        public List<Result> results { get; set; }
    }

    public class Hero
    {
        public int code { get; set; }
        public string status { get; set; }
        public string copyright { get; set; }
        public string attributionText { get; set; }
        public string attributionHTML { get; set; }
        public string etag { get; set; }
        public Data data { get; set; }
    }




}

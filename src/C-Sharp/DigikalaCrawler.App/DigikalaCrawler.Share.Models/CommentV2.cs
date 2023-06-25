﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace DigikalaCrawler.Share.Models
{
    // <auto-generated />
    //
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using CodeBeautify;
    //
    //    var welcome4 = Welcome4.FromJson(jsonString);

    public class CommentObjV1
    {
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("status")]
        public int Status;

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("data")]
        public CommentData Data;
    }

    public class CommentData
    {
        [JsonProperty("ratings", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("ratings")]
        public List<Rating> Ratings;

        [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("comments")]
        public List<Comment> Comments;

        [JsonProperty("intent_data", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("intent_data")]
        public List<IntentDatum> IntentData;

        [JsonProperty("sort", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("sort")]
        public Sort Sort;

        [JsonProperty("sort_options", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("sort_options")]
        public List<SortOption> SortOptions;

        [JsonProperty("media_comments", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("media_comments")]
        public List<MediaComment> MediaComments;

        [JsonProperty("pager", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("pager")]
        public Pager Pager;
    }

    public class Comment
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("id")]
        public int? Id;

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("title")]
        public string Title;

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("body")]
        public string Body;

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("created_at")]
        public string CreatedAt;

        [JsonProperty("rate", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("rate")]
        public double? Rate;

        [JsonProperty("reactions", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("reactions")]
        public Reactions Reactions;

        [JsonProperty("files", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("files")]
        public List<FileComment> Files;

        [JsonProperty("recommendation_status", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("recommendation_status")]
        public string RecommendationStatus;

        [JsonProperty("is_buyer", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("is_buyer")]
        public bool? IsBuyer;

        [JsonProperty("user_name", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("user_name")]
        public string UserName;

        [JsonProperty("is_anonymous", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("is_anonymous")]
        public bool? IsAnonymous;

        //[JsonProperty("purchased_item", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("purchased_item")]
        //public object PurchasedItem;

        [JsonProperty("advantages", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("advantages")]
        public List<string> Advantages;

        [JsonProperty("disadvantages", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("disadvantages")]
        public List<string> Disadvantages;
    }

    public class FileComment
    {
        [JsonProperty("storage_ids", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("storage_ids")]
        public List<string> StorageIds;

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("url")]
        public List<string> Url;

        [JsonProperty("thumbnail_url", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("thumbnail_url")]
        public List<string> ThumbnailUrl;

        [JsonProperty("temporary_id", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("temporary_id")]
        public object TemporaryId;

        [JsonProperty("webp_url", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("webp_url")]
        public object WebpUrl;
    }

    public class Grade
    {
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("label")]
        public string Label;

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("color")]
        public string Color;
    }

    public class IntentDatum
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("title")]
        public string Title;

        [JsonProperty("number_of_comments", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("number_of_comments")]
        public int? NumberOfComments;

        [JsonProperty("tag_data", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("tag_data")]
        public TagData TagData;

        [JsonProperty("tag_percentage", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("tag_percentage")]
        public TagPercentage TagPercentage;

        [JsonProperty("productId", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("productId")]
        public int? ProductId;

        [JsonProperty("intentId", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("intentId")]
        public int? IntentId;
    }

    public class MediaComment
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("id")]
        public int? Id;

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("title")]
        public string Title;

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("body")]
        public string Body;

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("created_at")]
        public string CreatedAt;

        [JsonProperty("rate", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("rate")]
        public double Rate;

        [JsonProperty("reactions", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("reactions")]
        public Reactions Reactions;

        [JsonProperty("files", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("files")]
        public List<FileComment> Files;

        [JsonProperty("recommendation_status", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("recommendation_status")]
        public string RecommendationStatus;

        [JsonProperty("is_buyer", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("is_buyer")]
        public bool? IsBuyer;

        [JsonProperty("user_name", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("user_name")]
        public string UserName;

        [JsonProperty("is_anonymous", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("is_anonymous")]
        public bool? IsAnonymous;

        //[JsonProperty("purchased_item", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("purchased_item")]
        //public PurchasedItem PurchasedItem;

        [JsonProperty("advantages", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("advantages")]
        public List<string> Advantages;

        [JsonProperty("disadvantages", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("disadvantages")]
        public List<string> Disadvantages;
    }

    public class Pager
    {
        [JsonProperty("current_page", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("current_page")]
        public int? currentPage;

        [JsonProperty("total_pages", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("total_pages")]
        public int total_pages;

        [JsonProperty("total_items", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("total_items")]
        public int? totalItems;
    }

    public class Properties
    {
        [JsonProperty("is_trusted", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("is_trusted")]
        public bool? IsTrusted;

        [JsonProperty("is_official", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("is_official")]
        public bool? IsOfficial;

        [JsonProperty("is_roosta", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("is_roosta")]
        public bool? IsRoosta;

        [JsonProperty("is_new", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("is_new")]
        public bool? IsNew;
    }

    public class PurchasedItem
    {
        [JsonProperty("seller", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("seller")]
        public Seller Seller;
    }

    public class Rating
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("id")]
        public int? Id;

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("title")]
        public string Title;

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("value")]
        public double? Value;
    }

    public class Rating2
    {
        [JsonProperty("total_rate", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("total_rate")]
        public object TotalRate;

        [JsonProperty("total_count", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("total_count")]
        public int? TotalCount;

        [JsonProperty("commitment", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("commitment")]
        public object Commitment;

        [JsonProperty("no_return", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("no_return")]
        public object NoReturn;

        [JsonProperty("on_time_shipping", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("on_time_shipping")]
        public object OnTimeShipping;
    }

    public class Reactions
    {
        [JsonProperty("likes", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("likes")]
        public int? Likes;

        [JsonProperty("dislikes", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("dislikes")]
        public int? Dislikes;
    }

    

    public class Seller
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("id")]
        public int? Id;

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("title")]
        public string Title;

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("code")]
        public string Code;

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("url")]
        public string Url;

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("rating")]
        public Rating Rating;

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("properties")]
        public Properties Properties;

        [JsonProperty("stars", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("stars")]
        public object Stars;

        [JsonProperty("grade", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("grade")]
        public Grade Grade;
    }

    public class Sort
    {
        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("default")]
        public string Default;
    }

    public class SortOption
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("id")]
        public string Id;

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("title")]
        public string Title;
    }

    public class TagData
    {
        [JsonProperty("positive", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("positive")]
        public int? Positive;

        [JsonProperty("negative", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("negative")]
        public int? Negative;

        [JsonProperty("neutral", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("neutral")]
        public int? Neutral;
    }

    public class TagPercentage
    {
        [JsonProperty("positive", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("positive")]
        public int? Positive;

        [JsonProperty("negative", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("negative")]
        public int? Negative;

        [JsonProperty("neutral", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonPropertyName("neutral")]
        public int? Neutral;
    }


}

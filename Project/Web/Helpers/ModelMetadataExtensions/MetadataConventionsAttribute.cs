using System;

namespace Web.Helpers.ModelMetadataExtensions
{
    public class MetadataConventionsAttribute : Attribute
    {
        public Type ResourceType { get; set; }
    }
}
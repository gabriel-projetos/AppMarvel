using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace App.JsonResolver
{
    //https://stackoverflow.com/questions/25950112/json-deserialize-to-constructed-protected-setter-array
    public class CustomContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasNonPublicSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasNonPublicSetter;
                }
            }

            return prop;
        }
    }
}

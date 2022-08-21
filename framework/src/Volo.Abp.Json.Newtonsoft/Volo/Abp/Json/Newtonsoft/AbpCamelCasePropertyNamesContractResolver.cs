﻿using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.Newtonsoft;

public class AbpCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver, ITransientDependency
{
    private readonly Lazy<AbpJsonIsoDateTimeConverter> _dateTimeConverter;

    public AbpCamelCasePropertyNamesContractResolver(IServiceProvider serviceProvider)
    {
        _dateTimeConverter = new Lazy<AbpJsonIsoDateTimeConverter>(
            serviceProvider.GetRequiredService<AbpJsonIsoDateTimeConverter>,
            true
        );

        NamingStrategy = new CamelCaseNamingStrategy
        {
            ProcessDictionaryKeys = false
        };
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (AbpJsonIsoDateTimeConverter.ShouldNormalize(member, property))
        {
            property.Converter = _dateTimeConverter.Value;
        }

        return property;
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using SystemCommonLibrary.Enums;
using SystemCommonLibrary.Reflect;

namespace SystemCommonLibrary.Data.DataEntity
{
    public static class EntitySchemaReader
    {
        public static EntitySchema GetSchema<T>()
        {
            var type = typeof(T);
            var schema = new EntitySchema();

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var attrColumn = (ColumnAttribute)property.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();
                var attrEditor = (EditorAttribute)property.GetCustomAttributes(typeof(EditorAttribute), true).FirstOrDefault();
                var attrKey = (KeyAttribute)property.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault();
                var attrEntityKey = (EntityKeyAttribute)property.GetCustomAttributes(typeof(EntityKeyAttribute), true).FirstOrDefault();
                var attrForeign = (ForeignAttribute)property.GetCustomAttributes(typeof(ForeignAttribute), true).FirstOrDefault();

                if (attrColumn != null || attrEditor != null 
                    || attrKey != null || attrEntityKey != null
                    || attrForeign != null)
                {
                    var col = new EntityColumn(property.Name);
                    schema.Columns.Add(col);

                    if (attrColumn != null)
                    {
                        col.Name = attrColumn.Name;
                        col.Hidden = attrColumn.Hidden;
                        col.Formatter = attrColumn.Formatter;
                    }

                    if (attrEditor != null)
                    {
                        col.Editable = attrEditor.Editable;
                        col.Editor = attrEditor.EditorType;
                        col.Length = attrEditor.Length;
                        col.Required = attrEditor.Required;
                        col.Items = attrEditor.Items ?? GetEnumItems(attrEditor.EditorType, property);
                        col.Min = attrEditor.Min;
                        col.Max = attrEditor.Max;
                        col.Scale = attrEditor.Scale;
                    }

                    if (attrKey != null)
                    {
                        col.IsKey = true;
                    }

                    if (attrEntityKey != null)
                    {
                        col.IsEntityKey = true;
                    }

                    if (attrForeign != null)
                    {
                        col.Foreign = new ForeignEntity() {
                            On = attrForeign.Foreign,
                            Key = attrForeign.Key,
                            Display = attrForeign.Display
                        };
                    }
                }
            }

            return schema;
        }

        private static Dictionary<string, object> GetEnumItems(EditorType type, PropertyInfo property)
        {
            if ((type == EditorType.Checkbox || type == EditorType.List)
                && property.PropertyType.IsEnum)
            {
                var items = new Dictionary<string, object>();
                var values = property.PropertyType.GetEnumValues();
                foreach (var value in values)
                {
                    var name = ((Enum)value).GetDescription();

                    items.Add(name, (int)value);
                }

                return items;
            }
            else
                return null;
        }
    }
}

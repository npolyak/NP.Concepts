﻿// (c) Nick Polyak 2013 - http://awebpros.com/
// License: Code Project Open License (CPOL) 1.92(http://www.codeproject.com/info/cpol10.aspx)
//
// short overview of copyright rules:
// 1. you can use this framework in any commercial or non-commercial 
//    product as long as you retain this copyright message
// 2. Do not blame the author(s) of this software if something goes wrong. 
// 
// Also as a courtesy, please, mention this software in any documentation for the 
// products that use it.

using NP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NP.Concepts.Expressions
{
    public static class CompiledExpressionUtils
    {
        static DoubleParamMap<Type, string, Func<object, object>> _untypedGettersCache = 
            new DoubleParamMap<Type, string, Func<object, object>>();


        public static Func<object, object> GetUntypedCSPropertyGetterByObjType
        (
            this Type objType, 
            string propertyName
        )
        {
            Func<object, object> result;

            if (_untypedGettersCache.TryGetValue(objType, propertyName, out result))
            {
                return result;
            }

            ParameterExpression paramExpression = Expression.Parameter(typeof(object));
            UnaryExpression typedObjectExpression = Expression.Convert(paramExpression, objType);

            Expression propertyGetterExpression =
                Expression.Property(typedObjectExpression, propertyName);

            UnaryExpression valueCastExpression = Expression.Convert(propertyGetterExpression, typeof(object));

            result = Expression.Lambda<Func<object, object>>(valueCastExpression, paramExpression).Compile();

            _untypedGettersCache.AddKeyValue(objType, propertyName, result);

            return result;
        }

        public static Func<object, object> GetUntypedCSPropertyGetter
        (
            object obj,
            string propertyName
        )
        {
            return GetUntypedCSPropertyGetterByObjType(obj?.GetType(), propertyName);
        }

        //typed getters cache
        static DoubleParamMap<Type, string, object> _typedGettersCache =
            new DoubleParamMap<Type, string, object>();

        // returns property getter
        public static Func<TObject, TProperty> GetTypedCSPropertyGetter<TObject, TProperty>(string propertyName)
        {
            Type objType = typeof(TObject);
            object resultObj;

            if (_typedGettersCache.TryGetValue(objType, propertyName, out resultObj))
            {
                return resultObj as Func<TObject, TProperty>;
            }

            ParameterExpression paramExpression = Expression.Parameter(objType, "value");

            Expression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            Func<TObject, TProperty> result =
                Expression.Lambda<Func<TObject, TProperty>>
                (
                    propertyGetterExpression, 
                    paramExpression
                ).Compile();

            _typedGettersCache.AddKeyValue(objType, propertyName, result);

            return result;
        }

        static DoubleParamMap<Type, string, Action<object, object>> _untypedSettersCache =
            new DoubleParamMap<Type, string, Action<object, object>>();

        public static Action<object, object> GetUntypedCSPropertySetterByObjType
        (
            this Type objType, 
            string propertyName
        )
        {
            Action<object, object> result;

            if (_untypedSettersCache.TryGetValue(objType, propertyName, out result))
            {
                return result;
            }

            Type propertyType = objType.GetPropType(propertyName);

            ParameterExpression objParamExpression = Expression.Parameter(typeof(object));

            UnaryExpression objCastExpression = Expression.Convert(objParamExpression, objType);

            ParameterExpression propertyParamExpression = Expression.Parameter(propertyType, propertyName);

            ParameterExpression valueParamExpression = Expression.Parameter(typeof(object));
            UnaryExpression valueCastExpression = Expression.Convert(valueParamExpression, propertyType);

            MemberExpression propertyExpression = Expression.Property(objCastExpression, propertyName);

            BinaryExpression assignExpression = Expression.Assign(propertyExpression, valueCastExpression);

            result = Expression.Lambda<Action<object, object>>
            (
                 assignExpression,
                 objParamExpression,
                 valueParamExpression
            ).Compile();

            _untypedSettersCache.AddKeyValue(objType, propertyName, result);

            return result;
        }


        public static Action<object, object> GetUntypedCSPropertySetter
        (
            object obj,
            string propertyName
        )
        {
            if (obj == null)
                return null;

            return GetUntypedCSPropertySetterByObjType(obj.GetType(), propertyName);
        }


        public static Action<object, object> GetUntypedVoidSingleArgMethodByObjType
        (
            this Type objType, 
            string methodName
        )
        {
            Action<object, object> result;

            if (_untypedSettersCache.TryGetValue(objType, methodName, out result))
            {
                return result;
            }

            Type methodArgType = objType.GetMethodArgType(methodName);

            ParameterExpression objParamExpression = Expression.Parameter(typeof(object));

            UnaryExpression objCastExpression = Expression.Convert(objParamExpression, objType);

            ParameterExpression inputParamExpression = Expression.Parameter(methodArgType, methodName);

            ParameterExpression valueParamExpression = Expression.Parameter(typeof(object));
            UnaryExpression valueCastExpression = Expression.Convert(valueParamExpression, methodArgType);

            MethodCallExpression methodExpression = Expression.Call(objCastExpression, methodName, null, valueCastExpression);

            result = Expression.Lambda<Action<object, object>>
            (
                 methodExpression,
                 objParamExpression,
                 valueParamExpression
            ).Compile();

            _untypedSettersCache.AddKeyValue(objType, methodName, result);

            return result;
        }


        static DoubleParamMap<Type, string, object> _typedSettersCache =
            new DoubleParamMap<Type, string, object>();

        // in this function we figure out the property type from the object itself
        public static Action<TObject, object> GetTypedCSPropertySetter<TObject>
        (
            this TObject obj, 
            string propertyName
        )
        {

            Type objType = obj.GetType();

            object resultObj;
            if (_typedSettersCache.TryGetValue(objType, propertyName, out resultObj))
            {
                return resultObj as Action<TObject, object>;
            }

            Type propertyType = objType.GetPropType(propertyName);

            ParameterExpression objParamExpression = 
                Expression.Parameter(objType);

            ParameterExpression propertyParamExpression = 
                Expression.Parameter(typeof(object), propertyName);

            UnaryExpression propertyCastExpression = 
                Expression.Convert(propertyParamExpression, propertyType);

            MemberExpression propertyExpression = 
                Expression.Property(objParamExpression, propertyName);

            BinaryExpression assignExpression = 
                Expression.Assign(propertyExpression, propertyCastExpression);

            Action<TObject, object> result = 
                Expression.Lambda<Action<TObject, object>>
            (
                assignExpression, objParamExpression, propertyParamExpression
            ).Compile();

            _typedSettersCache.AddKeyValue(objType, propertyName, result);

            return result;
        }


        static DoubleParamMap<Type, string, object> _fullyTypedSettersCache =
            new DoubleParamMap<Type, string, object>();

        // returns property setter:
        public static Action<TObject, TProperty> 
            GetFullyTypedCSPropertySetter<TObject, TProperty>(string propertyName)
        {
            Type objType = typeof(TObject);

            object resultObj;
            if(_fullyTypedSettersCache.TryGetValue(objType, propertyName, out resultObj))
            {
                return resultObj as Action<TObject, TProperty>;
            }

            ParameterExpression objParamExpression = Expression.Parameter(objType);

            ParameterExpression propertyParamExpression = Expression.Parameter(typeof(TProperty), propertyName);

            MemberExpression propertyGetterExpression = Expression.Property(objParamExpression, propertyName);

            BinaryExpression assignExpression = Expression.Assign(propertyGetterExpression, propertyParamExpression);

            Action<TObject, TProperty> result = Expression.Lambda<Action<TObject, TProperty>>
            (
                assignExpression, objParamExpression, propertyParamExpression
            ).Compile();

            _fullyTypedSettersCache.AddKeyValue(objType, propertyName, result);

            return result;
        }
    }
}

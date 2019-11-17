using NP.Utilities;
using NP.Utilities.BasicInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NP.Concepts.ComponentFolders
{
    public class ComponentIdWithDisplayMetadata<TId, TMetaData> :  
        IMatchable,
        IComponentMetaDataContainer<TMetaData>,
        INotifyPropertyChanged
        where TId : INameContainer
        where TMetaData : class, IComponentDisplayMetadata
    {
        internal static Dictionary<TId, TMetaData> MetadataMap { get; } =
            new Dictionary<TId, TMetaData>();

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsFolder => false;

        public TId TheComponentId { get; }

        public TMetaData MetaData { get; }

        bool _isMatching = true;
        public bool IsMatching
        {
            get => _isMatching;
            private set
            {
                if (_isMatching == value)
                    return;

                _isMatching = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMatching)));
            }
        }

        public ComponentIdWithDisplayMetadata(TId componentId)
        {
            MetaData = componentId.GetDisplayMetadata<TId, TMetaData>();
            TheComponentId = componentId;
        }

        public void CheckMatching(string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                IsMatching = true;
            }

            IsMatching = MetaData.DisplayName?.ToLower()?.Contains(str?.ToLower()) == true;
        }
    }

    public static class ComponentIdExtensions
    {
        public static void AddDisplayMetaData<TId, TMetaData>
        (
            this TId componentId,
            TMetaData metaData
        )
            where TId : INameContainer
            where TMetaData : class, IComponentDisplayMetadata
        {
            ComponentIdWithDisplayMetadata<TId, TMetaData>.MetadataMap[componentId] = metaData;
        }


        public static TMetaData GetDisplayMetadata<TId, TMetaData>(this TId componentId) 
            where TId : INameContainer
            where TMetaData : class, IComponentDisplayMetadata
        {
            if ( ComponentIdWithDisplayMetadata<TId, TMetaData>.MetadataMap.TryGetValue
                 (
                     componentId,
                     out TMetaData md))
            {
                return md;
            }

            return null;
        }
    }
}

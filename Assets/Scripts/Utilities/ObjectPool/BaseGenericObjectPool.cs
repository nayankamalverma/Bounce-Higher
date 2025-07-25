﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace BounceDash.Scripts.Utilities
{
    public class BaseGenericObjectPool<T>
    {
        public List<PooledItem<T>> pooledItems = new List<PooledItem<T>>();

        public GameObject GetItem(T type)
        {
            if (pooledItems.Count > 0)
            {
                PooledItem<T> item = pooledItems.Find(item => !item.isUsed && item.type.Equals(type));
                if (item != null)
                {
                    item.isUsed = true;
                    item.Item.SetActive(true);
                    return item.Item;
                }
            }

            return CreateNewPooledItem(type);
        }

        private GameObject CreateNewPooledItem(T type)
        {
            PooledItem<T> newItem = new PooledItem<T>();
            newItem.Item = CreateItem(type);
            newItem.type = type;
            newItem.isUsed = true;
            pooledItems.Add(newItem);
            return newItem.Item;
        }

        protected virtual GameObject CreateItem(T type)
        {
            throw new NotImplementedException("CreateItem() method not implemented in derived class");
        }

        public virtual void ReturnItem(PooledItem<T> item)
        {
            foreach (Transform child in item.Item.transform)
            {
                child.gameObject.SetActive(true);
            }
            item.Item.SetActive(false);
            item.isUsed = false;
        }

        public void ReturnAllItem()
        {
            foreach (var item in pooledItems)
            {
                ReturnItem(item);
            }
        }

    }
    public class PooledItem<T>
    {
        public GameObject Item;
        public T type;
        public bool isUsed;
    }
}
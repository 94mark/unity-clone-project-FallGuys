using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace DevionGames.UIWidgets
{
	public class UIContainer<T> : UIWidget where T : class
	{

        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>(base.Callbacks);
                callbacks.Add("OnAddItem");
                callbacks.Add("OnRemoveItem");
                return callbacks.ToArray();
            }
        }

        [Header ("Behaviour")]
        /// <summary>
        /// Sets the container as dynamic. Slots are instantiated at runtime.
        /// </summary>
        [SerializeField]
        protected bool m_DynamicContainer = false;
        /// <summary>
        /// The parent transform of slots. 
        /// </summary>
        [SerializeField]
        protected Transform m_SlotParent;
        /// <summary>
        /// The slot prefab. This game object should contain the Slot component or a child class of Slot. 
        /// </summary>
        [SerializeField]
        protected GameObject m_SlotPrefab;

        protected List<UISlot<T>> m_Slots = new List<UISlot<T>>();
        /// <summary>
        /// Collection of slots this container is holding
        /// </summary>
        public ReadOnlyCollection<UISlot<T>> Slots
        {
            get
            {
                return this.m_Slots.AsReadOnly();
            }
        }

        protected List<T> m_Collection;
  
        protected override void OnAwake ()
		{
			base.OnAwake ();
            this.m_Collection = new List<T>();
			RefreshSlots ();
		}

        /// <summary>
        /// Adds a new item to a free or dynamicly created slot in this container.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>True if item was added.</returns>
        public virtual bool AddItem(T item)
        {
            UISlot<T> slot = null;
            if (CanAddItem(item, out slot, true))
            {
                
                ReplaceItem(slot.Index, item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the item at index. Sometimes an item requires more then one slot(two-handed weapon), this will remove the item with the extra slots.
        /// </summary>
        /// <param name="index">The slot index where to remove the item.</param>
        /// <returns>Returns true if the item was removed.</returns>
        public virtual bool RemoveItem(int index)
        {
            if (index < this.m_Slots.Count)
            {
                UISlot<T> slot = this.m_Slots[index];
                T item = slot.ObservedItem;

                if (item != null)
                {
                    this.m_Collection.Remove(item);
                    slot.ObservedItem = null;
                  
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Replaces the items at index and returns the previous item.
        /// </summary>
        /// <param name="index">Index of slot to repalce.</param>
        /// <param name="item">Item to replace with.</param>
        /// <returns></returns>
        public virtual T ReplaceItem(int index, T item)
        {
            
            if (index < this.m_Slots.Count)
            {
                UISlot<T> slot = this.m_Slots[index];
                if (!slot.CanAddItem(item)) {
                    return item;
                }
                
                if (item != null)
                {
                    this.m_Collection.Add(item);

                    T current = slot.ObservedItem;
                    if (current != null)
                    {
                        RemoveItem(slot.Index);
                    }
                    slot.ObservedItem = item;
                    return current;
                }
            }
            return item;
        }

        /// <summary>
        /// Checks if the item can be added to this container.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <param name="slot">Required or next free slot</param>
        /// <param name="createSlot">Should a slot be created if the container is dynamic.</param>
        /// <returns>Returns true if the item can be added.</returns>
        public virtual bool CanAddItem(T item, out UISlot<T> slot, bool createSlot = false)
        {
            slot = null;
            if (item == null) { return true; }

            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                if (this.m_Slots[i].IsEmpty && this.m_Slots[i].CanAddItem(item))
                {
                    slot = this.m_Slots[i];
                    return true;
                }
            }

            if (this.m_DynamicContainer)
            {
                if (createSlot)
                {
                    slot = CreateSlot();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Refreshs the slot list and reorganize indices
        /// </summary>
        public void RefreshSlots()
        {
            if (this.m_DynamicContainer && this.m_SlotParent != null)
            {
                this.m_Slots = this.m_SlotParent.GetComponentsInChildren<UISlot<T>>(true).ToList();
                this.m_Slots.Remove(this.m_SlotPrefab.GetComponent<UISlot<T>>());
            }
            else
            {
                this.m_Slots = GetComponentsInChildren<UISlot<T>>(true).ToList();
            }

            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                UISlot<T> slot = this.m_Slots[i];
                slot.Index = i;
                slot.Container = this;
            }
        }

        /// <summary>
        /// Creates a new slot
        /// </summary>
        protected virtual UISlot<T> CreateSlot()
        {
            if (this.m_SlotPrefab != null && this.m_SlotParent != null)
            {
                GameObject go = (GameObject)Instantiate(this.m_SlotPrefab);
                go.SetActive(true);
                go.transform.SetParent(this.m_SlotParent, false);
                UISlot<T> slot = go.GetComponent<UISlot<T>>();
                this.m_Slots.Add(slot);
                slot.Index = Slots.Count - 1;
                slot.Container = this;
                return slot;
            }
            Debug.LogWarning("Please ensure that the slot prefab and slot parent is set in the inspector.");
            return null;
        }

        /// <summary>
        /// Destroy the slot and reorganize indices.
        /// </summary>
        /// <param name="slotID">Slot I.</param>
        protected virtual void DestroySlot(int index)
        {
            if (index < this.m_Slots.Count)
            {
                DestroyImmediate(this.m_Slots[index].gameObject);
                RefreshSlots();
            }
        }
    }
}
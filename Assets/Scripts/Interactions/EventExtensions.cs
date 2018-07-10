using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Interactions
{
	public static class EventExtensions {

		public static void AddListener(this GameObject gameObject,
			EventTriggerType eventTriggerType,
			UnityAction action) {
			// get the EventTrigger component; if it doesn't exist, create one and add it
			EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>()
			                            ?? gameObject.AddComponent<EventTrigger>();

			// check to see if the entry already exists
			EventTrigger.Entry entry;
			entry = eventTrigger.triggers.Find(e => e.eventID == eventTriggerType);

			if (entry == null) {
				// if it does not, create and add it
				entry = new EventTrigger.Entry {eventID = eventTriggerType};

				// add the entry to the triggers list
				eventTrigger.triggers.Add(entry);
			}

			// add the callback listener
			entry.callback.AddListener(_ => action());
		}

	}
}
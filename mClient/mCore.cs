using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClient
{
    public static class mCore
    {
        private static CallBackEvent Event;             // For sending events to the UI

        public static void Debug(Guid clientId)
        {
            Event m2 = new Event(clientId, EventType.EVENT_LOG, "0", new object[0]);
            Event(m2);
        }

        public static void Init(CallBackEvent e)
        {
            Event = e;
        }

        public static void SendError(Guid clientId, string msg)
        {
            Event m2 = new Event(clientId, EventType.EVENT_ERROR, "0", new object[1] { msg });
            Event(m2);
        }

        public static void SendEvent(Event e)
        {
            if (Event != null)
                Event(e);
        }
    }



    public class Event
    {
        public Guid ClientId;
        public EventType eventType;
        public string eventTime;
        public object[] eventArgs;

        public Event(Guid clientId, EventType type, string time, params object[] parms)
        {
            ClientId = clientId;
            eventType = type;
            eventTime = time;
            eventArgs = parms;
        }
    }

    public enum EventType
    {
        EVENT_REALMLIST,
        EVENT_CHARLIST,
        EVENT_ADD_OBJECT,
        EVENT_DEL_OBJECT,
        EVENT_UDT_OBJECT,
        EVENT_LOG,
        EVENT_CHAT_MSG,
        EVENT_ERROR,
        EVENT_DISCONNECT_LS,
        EVENT_DISCONNECT_WS
    }

    // Delegates - Used to make calls to the UI from this .dll
    public delegate void CallBackEvent(Event e);

}

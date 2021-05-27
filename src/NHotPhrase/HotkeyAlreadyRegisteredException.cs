using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace NHotPhrase
{
    [Serializable]
    public class HotkeyAlreadyRegisteredException : Exception
    {
        public HotkeyAlreadyRegisteredException(string name, Exception inner) : base(inner.Message, inner)
        {
            Name = name;
            HResult = Marshal.GetHRForException(inner);
        }

        public HotkeyAlreadyRegisteredException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            Name = (string) info.GetValue("_name", typeof (string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_name", Name);
        }

        public string Name { get; }
    }
}

using System;
using System.ComponentModel;
using System.Json;
using System.Runtime.CompilerServices;

namespace IoT.Device.Lumi.Gateway
{
    public abstract class LumiSubDevice : INotifyPropertyChanged
    {
        private string sid;
        private string model;
        private int id;

        public abstract string ModelName { get;}

        public LumiSubDevice(string sid, int id)
        {
            this.sid = sid;
            this.id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return $"{{\"model\": \"{ModelName}\", \"sid\": \"{sid}\", \"short_id\": {id}}}";
        }

        internal protected abstract void UpdateState(JsonValue jsonValue);
    }
}
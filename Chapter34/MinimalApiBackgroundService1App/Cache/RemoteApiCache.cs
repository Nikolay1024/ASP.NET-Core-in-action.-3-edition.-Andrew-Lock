using MinimalApiBackgroundService1App.Models;

namespace MinimalApiBackgroundService1App.Cache
{
    public class RemoteApiCache
    {
        private List<Album>? _albums;
        public List<Album>? Albums
        {
            get => _albums;
            set => Interlocked.Exchange(ref _albums, value);
        }
    }
}

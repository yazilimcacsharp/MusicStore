namespace MusicStore.Models
{
    public partial class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //Path: dosya yolu 
        //türü: string olsun
        //ve bu alan kategorilerin fotograf yolunu tutsun.
        public string Path { get; set; }
        public List<Album> Albums { get; set; } //o türden birden fazla albüm olabilir.
    }
}

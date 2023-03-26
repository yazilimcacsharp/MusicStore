tur tablosuna fotografların yolunu ekledik.

update Genres set Path='assets/images/'+lower(Name)+'.jpg' -- fotograf yollarını muzik türleri ile aynı isimde kaydettiğimiz için bu kod işimize yaradı.

Albums içerisindekini albumArtUrl bilgisini aşağıdaki gibi düzenledik.
  update Albums set AlbumArtUrl='assets/images/placeholder.jpg'


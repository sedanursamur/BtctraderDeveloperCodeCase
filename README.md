# BtctraderDeveloperCodeCase
Düzenli Bitcoin Alım Talimatı: Kullanıcıların her ayın belirledikleri bir gününde düzenli olarak bitcoin alımını talep edebilecekleri bir sistem kurgulanması isteniyor.

Gereksinimler
● Kullanıcı ayın 1-28 günleri arası için talimat verebilir.
● Kullanıcı hangi kanallardan bilgilendirilmek istediğini seçmelidir. Kullanıcı birden
fazla seçim yapabilir.
○ SMS
○ Email
○ Push Notification
● Kullanıcı minimum 100 TL’lik maksimum 20.000 TL’lik talimat verebilir.
● Bir kullanıcıya ait sadece 1 tane aktif talimat olabilir.
● Kullanıcı talimatını iptal edebilir.
● Kullanıcı verilen talimatı görüntüleyebilir.
● Kullanıcı iptal ettiği talimatları filtreleyip listeleyebilir.
● Kullanıcı talimatına ait bildirim kanallarını listeleyebilir.
● Talimat başarılı bir şekilde verildikten sonra kullanıcı bildirim almak istediği
kanaldan bilgilendirilmelidir. Bu noktada bilgilendirme işlemi için hayali bir http
isteği ile örnekleme yapılması beklenmektedir.
● Yapılan her bilgilendirme işlemi database seviyesinde loglanmalıdır. Hangi talimat
için hangi kanaldan ne zaman hangi bilgilendirme yazısı ile yapıldı sorularına cevap
verilebilmelidir.

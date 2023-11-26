# BtctraderDeveloperCodeCase
Düzenli Bitcoin Alım Talimatı: Kullanıcıların her ayın belirledikleri bir gününde düzenli olarak bitcoin alımını talep edebilecekleri bir sistem kurgulanması isteniyor.

# Gereksinimler
 ● Kullanıcı ayın 1-28 günleri arası için talimat verebilir.
<br> ● Kullanıcı hangi kanallardan bilgilendirilmek istediğini seçmelidir. Kullanıcı birden
fazla seçim yapabilir.
#### ○ SMS
#### ○ Email
#### ○ Push Notification
<br> ● Kullanıcı minimum 100 TL’lik maksimum 20.000 TL’lik talimat verebilir.
<br> ● Bir kullanıcıya ait sadece 1 tane aktif talimat olabilir.
<br> ● Kullanıcı talimatını iptal edebilir.
<br> ● Kullanıcı verilen talimatı görüntüleyebilir.
<br> ● Kullanıcı iptal ettiği talimatları filtreleyip listeleyebilir.
<br> ● Kullanıcı talimatına ait bildirim kanallarını listeleyebilir.
<br> ● Talimat başarılı bir şekilde verildikten sonra kullanıcı bildirim almak istediği
kanaldan bilgilendirilmelidir. Bu noktada bilgilendirme işlemi için hayali bir http
isteği ile örnekleme yapılması beklenmektedir.
<br> ● Yapılan her bilgilendirme işlemi database seviyesinde loglanmalıdır. Hangi talimat
için hangi kanaldan ne zaman hangi bilgilendirme yazısı ile yapıldı sorularına cevap
verilebilmelidir.

<div align="center">

# 📰 Gazetary

### ASP.NET Core ile geliştirilmiş, Identity tabanlı kullanıcı yönetimi bulunan  
### yapay zeka destekli modern haber & blog platformu

<br/>

<img src="https://img.shields.io/badge/.NET-8.0-black"/>
<img src="https://img.shields.io/badge/ASP.NET_CORE-MVC-purple"/>
<img src="https://img.shields.io/badge/EF_CORE-ORM-green"/>
<img src="https://img.shields.io/badge/SQL-SERVER-blue"/>
<img src="https://img.shields.io/badge/IDENTITY-Auth-darkblue"/>
<img src="https://img.shields.io/badge/FluentValidation-Validation-orange"/>
<img src="https://img.shields.io/badge/MailKit-E--Mail-blue"/>
<img src="https://img.shields.io/badge/AI-Moderation-black"/>
<img src="https://img.shields.io/badge/IMemoryCache-Performance-red"/>

</div>

---

## 📌 Proje Hakkında

Gazetary, kullanıcıların güncel haberleri takip edebildiği, blog yazılarını okuyabildiği, yorum yapabildiği ve admin paneli üzerinden içeriklerin yönetilebildiği modern bir haber & blog platformudur.

Proje; çok katmanlı mimari, ASP.NET Core Identity, SEO odaklı içerik yönetimi, RSS haber çekme sistemi, yapay zeka destekli yorum kontrolü ve cache mekanizması gibi yapılarla geliştirilmiştir.

Amaç yalnızca statik bir haber sitesi yapmak değil; kullanıcı yönetimi, admin paneli, otomatik veri çekme, güvenlik, performans ve sürdürülebilir mimariyi bir araya getiren gerçek bir web uygulaması oluşturmaktır.

---

## ⚙️ Öne Çıkan Özellikler

### 🔐 ASP.NET Core Identity ile Kullanıcı Yönetimi
- Kullanıcı kayıt sistemi  
- Kullanıcı giriş sistemi  
- Email doğrulama sistemi  
- Doğrulama kodu ile hesap aktivasyonu  
- Şifre sıfırlama (Forgot Password) sistemi  
- Email üzerinden güvenli şifre yenileme  
- Profil sayfası  
- Güvenli oturum yönetimi  

---

### 🤖 AI Destekli Yorum Moderasyonu
- Kullanıcı yorumları yapay zeka ile analiz edilir
- Uygunsuz, spam ve zararlı içerikler kontrol edilir
- Platform kalitesi ve içerik güvenliği artırılır

---

### 📰 RSS ile Otomatik Haber Çekme Sistemi
- Admin panelinden RSS kaynakları yönetilebilir
- Farklı haber kaynaklarından içerikler çekilebilir
- Haberler admin panelinde listelenir
- İçerik üretim süreci otomatikleştirilir

---

### 💱 Canlı Döviz Kuru Entegrasyonu
- TCMB XML servisi üzerinden USD ve EUR kurları çekilir
- Veriler belirli süreyle cache’lenir
- Gereksiz servis istekleri engellenir

---

### 🌤️ Hava Durumu Entegrasyonu
- Open-Meteo API üzerinden İstanbul anlık hava durumu alınır
- Sıcaklık verisi kullanıcı arayüzünde gösterilir
- Veriler cache mekanizması ile optimize edilir

---

### 🔍 SEO Odaklı İçerik Yönetimi
- Blog yazıları SEO uyumlu şekilde eklenebilir
- Başlık, açıklama ve içerik yapısı SEO’ya uygun hazırlanır
- Sayfalar arama motorlarına uygun yapıdadır

---

### 🎨 Modern UI (Google StitchAI)
- Tüm UI tasarımı Google StitchAI ile oluşturulmuştur  
- Minimal ve modern tasarım dili ile kullanıcı deneyimi ön plana çıkarılmıştır  
- Görsel hiyerarşi ve içerik okunabilirliği optimize edilmiştir  
- Responsive yapı sayesinde tüm cihazlarda sorunsuz çalışır  

---

### 📄 Sayfalama Sistemi
- Blog listeleri
- Kategori sayfaları
- Admin listeleme ekranları  
üzerinde sayfalama yapısı kullanılmıştır.

---

### ⚡ Cache ve Performans Optimizasyonu
- IMemoryCache ile sık kullanılan veriler cache’lenir
- Blog yazıları, kategori içerikleri, en çok okunanlar ve günlük haberler optimize edilir
- Veritabanı yükü azaltılır
- Daha hızlı sayfa yanıtları hedeflenir

---

## 🏗️ Proje Mimarisi

Proje çok katmanlı mimari yapısı ile geliştirilmiştir.

- UI / Presentation Layer
- Business Layer
- Data Access Layer
- Entity Layer
- DTO Layer

Bu mimari sayesinde proje daha okunabilir, sürdürülebilir ve genişletilebilir hale getirilmiştir.

---

## 🛠️ Kullanılan Teknolojiler

- ASP.NET Core 8
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server
- FluentValidation
- MailKit
- IMemoryCache
- HttpClient
- RSS Feed
- TCMB XML Servisi
- OpenAI API

---

# Proje Görselleri

## Anasayfa

<p align="center">
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a1.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a2.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a3.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a4.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a5.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a6.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a7.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a8.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/a9.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/c1.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/contact.png" width="800"/>
</p>

---

## 🔐 Kullanıcı Giriş, Kayıt ve Email Doğrulama, Şifre Sıfırlama

<p align="center">
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/l1.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/r1.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/dogrulama.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/emailkod.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/sifres.png" width="800"/>
</p>

---

## 👤 Profil

<p align="center">
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/p1.png" width="800"/>
</p>

---

## 🚫 404 Sayfası

<p align="center">
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/404.png" width="800"/>
</p>

---

## ⚙️ Admin Paneli

<p align="center">
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/alogin.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/bloglist.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/blog_add.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/categorylist.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/commantlist.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/contactlist.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/imageslist.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/rsslist.png" width="800"/>
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/userlistnew.png" width="800"/>
</p>

---

## 📝 Yazı Detay Sayfası

<p align="center">
  <img src="https://github.com/furkanturkerr/Gazetary/blob/main/Gazetary_UI/wwwroot/images/d1.png" width="800"/>
</p>

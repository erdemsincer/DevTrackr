# 🚀 DevTrackr – AI Destekli Kişisel Geliştirici Dashboardu

**DevTrackr**, yazılım geliştiricilerin kodlama alışkanlıklarını, üretkenliklerini ve ilerlemelerini tek bir yerde takip edebileceği **AI destekli** bir kişisel dashboard platformudur.  
Platform, GitHub aktivitelerinizi analiz eder, Pomodoro ve Task takibi yapar, ve GPT-4 ile size özel haftalık raporlar oluşturur.

---

## 🎯 Amaç

- 🧠 Geliştiricilere haftalık olarak AI destekli geri bildirimler sunmak  
- 📊 GitHub üzerinden commit, repo, dil gibi verileri çekmek ve analiz etmek  
- ⏱ Pomodoro ve görev yönetimiyle üretkenliği takip etmek  
- 📅 Takvim görünümüyle zaman bazlı verileri görselleştirmek  
- ✅ Kişisel gelişimi veriyle desteklemek

---

## 🧩 Mikroservisler

| Servis                        | Açıklama                                                                 |
|------------------------------|--------------------------------------------------------------------------|
| **AuthService**              | JWT tabanlı kullanıcı kimlik doğrulama ve token üretimi                 |
| **UserService**              | Kullanıcı profili yönetimi (GitHub username, tema, bio vb.)             |
| **TaskService**              | Görev oluşturma, tamamlama, güncelleme işlemleri                        |
| **PomodoroService**          | Pomodoro oturumlarını başlatma, tamamlama ve istatistik yönetimi        |
| **ActivityService**          | GitHub API üzerinden repo, commit ve istatistik çekimi                  |
| **AiReportService**          | GPT-4 ile haftalık AI raporu oluşturma                                  |

---

## 🛠️ Kullanılan Teknolojiler

### Backend
- **ASP.NET Core 8.0**
- **PostgreSQL** (her servis için izole veritabanı)
- **MassTransit + RabbitMQ** (event tabanlı mesajlaşma)
- **Entity Framework Core**
- **Quartz.NET**
- **OpenAI GPT-4 API**
- **Docker + Docker Compose**

### Frontend
- **React** (Vite + TypeScript)
- **Tailwind CSS**
- **React Router**
- **Axios**
- **Chart.js**
- **React Context + JWT Storage**

---

## 🔐 Kimlik Doğrulama

Tüm kullanıcı işlemleri JWT ile güvence altındadır.  
Refresh token sistemiyle birlikte token doğrulaması yapılır.  
Tüm servisler token'dan gelen `userId` bilgisiyle çalışır.

---

## 📸 Ekran Görüntüleri


![Image](https://github.com/user-attachments/assets/26e21c71-b2d6-42b1-a0cc-34c0c33025ea)

![Image](https://github.com/user-attachments/assets/19a03c0a-5820-428d-94d1-3beb3ad92677)

![Image](https://github.com/user-attachments/assets/66d7e6e1-5e26-4983-8931-bd8fd55d58ec)


![Image](https://github.com/user-attachments/assets/d78ab99d-a7e8-4806-a96f-cbfaa9f7ba6e)


![Image](https://github.com/user-attachments/assets/40632898-739b-4979-85f2-d16247ba15c0)

![Image](https://github.com/user-attachments/assets/a3f640bd-1ee9-4a2c-83c2-ab627b843f49)

![Image](https://github.com/user-attachments/assets/19cde80a-a8c5-4e02-bae0-8f64e88568dd)

---


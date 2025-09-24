# 🧠 BrainScanAI

Automatska detekcija i klasifikacija tumora mozga korišćenjem vještačke inteligencije i mikroservisne arhitekture.

---

## 📌 Opis projekta

BrainScanAI je istraživačko-razvojni projekat koji koristi **duboko učenje (CNN)** za klasifikaciju MRI snimaka mozga u četiri kategorije:

* Gliom
* Meningiom
* Tumor hipofize
* Bez tumora

Sistem je organizovan kao **mikroservisna arhitektura**:

* **Python servis** trenira i pokreće CNN model za klasifikaciju MRI slika.
* **.NET Core backend** upravlja korisnicima, obradom i komunikacijom sa AI modelom.
* **gRPC** se koristi za brzu sinhronu komunikaciju između servisa.
* **RabbitMQ** omogućava asinhrono procesiranje zahtjeva.
* **React Native mobilna aplikacija** pruža interfejs za upload MRI snimaka i pregled rezultata.
* **PostgreSQL** služi za čuvanje korisničkih podataka, MRI snimaka i rezultata klasifikacije.
* **Docker** se koristi za kontejnerizaciju i deploy svih komponenti.

---

## ⚙️ Korišćene tehnologije

* **Python** (TensorFlow, Keras, OpenCV, NumPy)
* **.NET Core** (mikroservisi, Entity Framework)
* **gRPC** – sinhrona komunikacija
* **RabbitMQ** – asinhrona komunikacija
* **PostgreSQL** – baza podataka
* **React Native** – mobilna aplikacija (Android & iOS)
* **Docker** – kontejnerizacija i orkestracija

---

## 📊 Arhitektura sistema

![Arhitektura sistema](putanja_do_slike)

```
React Native App
        │
   [gRPC API]
        │
 ┌───────────────┐
 │   .NET Core   │   <───► PostgreSQL
 │  mikroservisi │
 └───────────────┘
        │
   [gRPC + RabbitMQ]
        │
   Python AI servis
        │
     CNN Model
```

---

## 🧩 CNN model

Arhitektura modela:

* Conv2D + MaxPooling (32 filtera)
* Conv2D + MaxPooling (64 filtera)
* Conv2D + MaxPooling (128 filtera)
* Flatten
* Dense (128 neurona, ReLU)
* Dropout (0.5)
* Dense izlazni sloj (softmax, 4 klase)

**Dataset:** [Brain Tumor MRI Dataset (Kaggle)](https://www.kaggle.com/datasets)

**Trening parametri:**

* Optimizator: Adam
* Funkcija gubitka: Categorical Crossentropy
* Batch size: 32
* Epochs: 50
* Callback: EarlyStopping, ModelCheckpoint
* Augmentacija: rotacija, zoom, translacija, horizontal flip

### 📈 Rezultati treniranja


---

## 🚀 Pokretanje projekta

### 1. Kloniranje repozitorija

```bash
git clone https://github.com/Zeljko12980/BrainScanAI.git
cd BrainScanAI
```

### 2. Pokretanje servisa kroz Docker

```bash
docker-compose up --build
```

### 3. Pokretanje mobilne aplikacije

```bash
cd client
npm install
npm run start
```

---

## ✅ Funkcionalnosti

* Upload MRI slika putem mobilne aplikacije
* Automatska klasifikacija tumora (4 klase)
* Pregled rezultata klasifikacije
* Skalabilna mikroservisna arhitektura
* Sinhrona i asinhrona obrada zahtjeva
* Sigurno čuvanje podataka u PostgreSQL bazi

---

## 📱 Prikaz aplikacije
![viber_image_2025-09-24_13-25-10-724](https://github.com/user-attachments/assets/652fd059-ed4a-4efe-a66d-64cc30c8b552)
![viber_image_2025-09-24_13-25-10-814](https://github.com/user-attachments/assets/4cdcf5bd-ae74-442c-9dbd-693ad53ccf10)
![viber_image_2025-09-24_13-25-10-890](https://github.com/user-attachments/assets/e65fd8f1-f450-4699-b4d9-24693fd42059)
![viber_image_2025-09-24_13-25-11-001](https://github.com/user-attachments/assets/43c95804-c1be-4c7f-90e3-5427f2698d71)
![viber_image_2025-09-24_13-25-11-082](https://github.com/user-attachments/assets/35fe91a4-b2dc-4d92-abef-f90f8fcbaefc)
![viber_image_2025-09-24_13-25-11-286](https://github.com/user-attachments/assets/d1b69e26-740a-4ec7-97db-6e9686a02b90)
![viber_image_2025-09-24_13-25-11-343](https://github.com/user-attachments/assets/195cde31-60d8-4baa-9380-010f1130b115)
![viber_image_2025-09-24_13-25-11-405](https://github.com/user-attachments/assets/ac9aee47-2ec2-4c9a-b1a6-1040ec87512e)
![viber_image_2025-09-24_13-25-11-471](https://github.com/user-attachments/assets/c9410fc4-b9ca-47df-9a71-01c419c6324a)
![viber_image_2025-09-24_13-25-11-471](https://github.com/user-attachments/assets/ae8a1429-e922-454e-8622-fd2cc2c54c96)
![viber_image_2025-09-24_13-25-11-578](https://github.com/user-attachments/assets/2b19dcfe-f2d1-44b6-8538-5100f5307a16)
![viber_image_2025-09-24_13-25-11-630](https://github.com/user-attachments/assets/5dfb8a4f-416f-4849-be33-63201d3cb977)
![viber_image_2025-09-24_13-25-11-684](https://github.com/user-attachments/assets/dac0919a-8bd4-49cb-a7be-53a8896e300b)
![viber_image_2025-09-24_13-25-11-684](https://github.com/user-attachments/assets/855d7a91-61f2-4ffe-88da-6135aa3a8f1c)
![viber_image_2025-09-24_13-24-45-088](https://github.com/user-attachments/assets/1817d481-e4e2-4ee1-9cdd-0b4d226624da)
![viber_image_2025-09-24_13-24-45-172](https://github.com/user-attachments/assets/41bb0f5d-b0fb-4f92-9c8a-76f3a79cc1e3)
![viber_image_2025-09-24_13-24-45-228](https://github.com/user-attachments/assets/b3d2996b-70ec-4942-abc7-082e4529189b)
![viber_image_2025-09-24_13-24-45-287](https://github.com/user-attachments/assets/a6f6af82-b0e3-48f7-912f-03518c68c0b2)
![viber_image_2025-09-24_13-24-45-341](https://github.com/user-attachments/assets/eb9e6c17-b8cd-480a-89c9-e20ce82bf907)
![viber_image_2025-09-24_13-24-45-477](https://github.com/user-attachments/assets/ca8481b2-c5da-4319-b7c1-3f21cfad34b0)
![viber_image_2025-09-24_13-24-45-577](https://github.com/user-attachments/assets/df491b72-6f72-4b9a-962c-659636343690)
![viber_image_2025-09-24_13-24-45-728](https://github.com/user-attachments/assets/9580390a-2cde-490f-a3b9-5b2c3bdd8d6c)



---

## 👨‍💻 Autori

* Željko Ikanović
* Aleksej Mutić


---

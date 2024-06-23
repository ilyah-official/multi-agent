# Team-Based Competitive Multi Agent System
Sistem yang melakukan interaksi antara satu agen dengan agen lainnya menggunakan reinforcement learning (MLAgents Unity). Buku panduan ada di file bernama "Manual_Guide.pdf" dan bisa didownload di bagian release v1.0.
![Screenshot](Thumbnail.png)

# Fitur Utama
1. Dua buah tim yaitu kuning dan biru
2. Sistem kolaborasi terjadi dalam satu tim (interaksi terjadi pada agen dalam satu tim)
3. Sistem kompetisi terjadi dalam dua tim (interaksi terjadi antar semua agen)
4. Sistem hukuman (punishment)
5. Reinforcement learning based

# Flow Chart secara keseluruhan
Berikut adalah gambar flow chart dari simulasi yang telah dibuat:
![Screenshot](Architecture.png)

Secara umum, flow chart ini terdiri dari tiga bagian utama. Bagian pertama adalah persiapan dua buah model agen. Bagian kedua adalah melatih kedua model agen dalam satu environment. Latihan dilakukan secara terpisah sehingga tidak langsung dilakukan duel. Bagian ketiga adalah bagian pengujian. Model 1 dan model 2 yang telah dilatih secara terpisah pada bagian kedua, akan dipakai dalam satu environment yang sama untuk melihat model yang memiliki kinerja terbaik.

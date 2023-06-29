# Bil424 Ödev1
 Emre Balaban 201101047
# Karşılaşılan Zorluklar
- Oyuncuyu rigidbody'e kuvvet uygularken oyuncu hareket etmek yerine moment oluşturup dönüyordu. Çözüm olarak: 
 > rigidbody.freezeRotation = true;
>
 kodu ile dönmeyi durdurarak hareket etmesini sağladım.

- Bir diğer zorluk ise sürekli aynı yönde hareket ettiğinde karakterin hızının çok hızlanması oldu. Bu sorunu da:
 > rb.velocity = new Vector3(0, rb.velocity.y, 0);
>
 koduyla, ne zaman hareket etmeye devam edecekse önceki kuvvetleri sıfırlayarak yapmasını sağladım.
 
- Karşılaştığım son zorluk ise animasyonlar oldu. Oyuncu hareket ederken yuvarlanma animasyonuna girip tekrardan yürüme animasyonuna giriyordu. Bu sorunu oyuncu yuvarlanırken yürümesini engelleyerek ve animasyona girerken trigger kullanmak yerine direk state'e giderek çözdüm. 
 Oyuncunun yuvarlanırken yürümesini engellemek aynı zamanda 2. sorunumu çözerkenki kullandığım
 > rb.velocity = new Vector3(0, rb.velocity.y, 0);
 >
  kodun yuvarlanma hareketini sıfırlamasını da engelledi.
  
# Oyun Videosu
 


https://github.com/EmreBlbn/Bil424Odev1-201101047/assets/78085195/eee94106-3e01-4bb0-9449-16138e920836



  

 

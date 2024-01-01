<div align="center">
  <a href="https://www.uit.edu.vn/" title="Trường Đại học Công nghệ Thông tin" target="_blank">
    <img src="https://www.uit.edu.vn/sites/vi/files/banner_uit_15.png">
  </a>
</div>

#
<!-- Badge -->
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
![HTML5](https://img.shields.io/badge/html5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/css3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white)
![Bootstrap](https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white)

# ỨNG DỤNG ĐA CHỨC NĂNG CHO NGƯỜI VIỆT - PROJECTX
ProjectX là một ứng dụng hoạt động đa nền tảng, tích hợp nhiều công cụ tiện ích, đa dạng và hiệu quả

# I. Mở đầu
- Trong thời đại số hóa ngày nay, việc sử dụng các công cụ trên máy tính để giải quyết các vấn đề trong cuộc sống và công việc là không thể thiếu. Tuy nhiên, các công cụ hiện nay thường được phát triển riêng lẻ, không tương thích với nhau, và không đáp ứng được nhu cầu đa dạng của người dùng. Để khắc phục những hạn chế này, nhóm chúng em đã nghiên cứu và phát triển một ứng dụng đa chức năng cho người Việt, có tên là ProjectX.
  
- ProjectX là một ứng dụng hoạt động đa nền tảng, tích hợp nhiều công cụ tiện ích, đa dạng và hiệu quả. ProjectX được phát triển dựa trên nền tảng công nghệ AI, cho phép thực hiện nhiều chức năng ấn tượng, như nhận dạng hình ảnh deepfake, ảnh căn cước công dân giả, chuyển đổi văn bản thành giọng nói, nhận dạng chữ trong ảnh v.v.
- ProjectX không chỉ mang lại sự tiện lợi cho người dùng, mà còn mở ra cơ hội để khám phá và tận dụng tối đa các công nghệ hiện đại.

# II. Mô tả
### 1. Ý tưởng
-	ProjectX sẽ giúp người dùng tăng năng suất làm việc, cải thiện trải nghiệm sử dụng máy tính, và bảo vệ an toàn thông tin cá nhân. Ứng dụng cung cấp một giao diện người dùng đơn giản và dễ sử dụng, cho phép người dùng tương tác với các tính năng và xem kết quả một cách thuận tiện.
  
-	ProjectX sẽ tích hợp nhiều công cụ khác nhau, từ những công cụ cơ bản như quản lý tập tin, quản lý clipboard, quản lý màn hình, đến những công cụ nâng cao như nhận dạng hình ảnh deepfake, ảnh căn cước công dân giả, chuyển đổi văn bản thành giọng nói, v.v.
### 2. Phân tích yêu cầu
-	Ứng dụng cần hoạt động mượt mà, không gây gián đoạn cho người dùng.
-	Dữ liệu của người dùng cần được bảo mật và không bị mất mát
-	Giao diện người dùng cần thân thiện dễ sử dụng
### 3. Công nghệ
ProjectX được xây dựng trên hai nền tảng chính là desktop và web, với các công nghệ sau:
  #### Destop App
  -	Hệ thống API: WPF
  -	IDE: Visual Studio 2022 
  -	Ngôn ngữ: C#
  -	Framework: .NET
  -	Database: MongoDB
  -	Công cụ quản lý mã nguồn: Git, Github.
  -	Công cụ quản lý cơ sở dữ liệu: MongoDB Compass
 #### Website
  - Front-end: HTML, CSS, JS
  -	Framework: React, Bootstrap
  -	Back-end: NodeJS
  -	Database: MongoDB
  -	Deployment: Vercel (Fontend cloud) & Railway (Infrastructure platform)
  -	Công cụ quản lý mã nguồn: Git, Github
  -	Test API: Postman
### 4. Các chức năng chính
  - **Text-to-speech**:
       <ul>
      <li> Giọng đọc: Người dùng có thể chọn giọng đọc thông qua một hộp combo box. Các lựa chọn bao gồm “Nữ miền Bắc”, “Nam miền Bắc”, “Nữ miền Nam”, và “Nam miền Nam”.
      <li> Tốc độ đọc: Người dùng có thể điều chỉnh tốc độ đọc thông qua một hộp combo box. Các lựa chọn bao gồm “Bình thường(1.0)”, “Rất chậm(0.8)”, “Chậm(0.9)”, “Nhanh(1.1)”, và “Rất nhanh(1.2)”.
      </li> Nút Đọc: Khi nhấn vào nút “Đọc”, ứng dụng sẽ bắt đầu đọc văn bản đã nhập.
      </li>	Nút Tải: Khi nhấn vào nút “Tải”, ứng dụng sẽ tải xuống file âm thanh của văn bản đã được đọc.
      </li> Nút Dừng: Khi nhấn vào nút “Dừng”, ứng dụng sẽ dừng việc đọc văn bản.
      <li> Nút Mở file: Khi nhấn vào nút “Mở file”, ứng dụng sẽ mở một hộp thoại cho phép người dùng chọn một file văn bản hoặc OCR văn bản từ file hình ảnh để đọc.
      <li> Hộp nhập văn bản: Người dùng có thể nhập văn bản muốn đọc vào hộp nhập văn bản.
      <li> Trạng thái: Ứng dụng hiển thị trạng thái hiện tại của quá trình đọc văn bản.
      </ul> 
      
  - **FaceMatch (Khớp khuôn mặt)**:
    Chức năng “Khớp khuôn mặt” trong ứng dụng cho phép người dùng tải lên hai hình ảnh và so sánh khuôn mặt trên hai hình ảnh đó. 
      <ul>
      <li> Người dùng nhấp vào nút “Tải lên ảnh 1” và “Tải lên ảnh 2” để tải lên hai hình ảnh cần so sánh khuôn mặt
      <li> Sau khi hai hình ảnh đã được tải lên, người dùng nhấp vào nút “Khớp khuôn mặt” để bắt đầu quá trình khớp khuôn mặt.
      <li> Kết quả của quá trình khớp khuôn mặt sẽ được hiển thị dưới nút “Khớp khuôn mặt”.
      <li> Hình ảnh được tải lên sẽ được hiển thị trong hai khung hình ảnh ở phía dưới của cửa sổ.
      </ul> 
      
  - **Translator (Dịch):** Cho phép người dùng dịch văn bản từ một ngôn ngữ này sang ngôn ngữ khác. 
      <ul>
      <li> Người dùng có thể nhập văn bản cần dịch vào text block. 
      <li> Người dùng có thể chọn ngôn ngữ mà họ muốn dịch văn bản sang từ combo box chứa danh sách các ngôn ngữ khác nhau.
      <li> Người dùng có thể nhấp vào nút “Dịch” để thực hiện việc dịch văn bản.
      <li> Người dùng cũng có thể mở tệp văn bản hoặc OCR văn bản từ tệp hình ảnh từ máy tính của người dùng để dịch bằng cách nhấp vào nút “Mở Tệp”.
      <li> Sau khi dịch, người dùng có thể tải xuống văn bản đã dịch dưới dạng tệp bằng cách nhấp vào nút “Tải Xuống Tệp”
      </ul> 

  - **GeminiAI Chat:**
Ứng dụng này cho phép người dùng giao tiếp với Gemini thông qua văn bản. Giao diện người dùng bao gồm Listbox để hiển thị cuộc trò chuyện và một hộp nhập liệu để người dùng nhập văn bản. Khi người dùng nhập văn bản, Gemini sẽ xử lý và trả lời, giao diện tương tự các Chatbot có trên thị trường.
### 5. Chức năng người dùng
-	Đăng ký tài khoản (Trên app & website)
-	Đăng nhập vào tài khoản hiện có
-	Đổi tên tài khoản
-	Đổi mật khẩu
-	Reset mật khẩu
-	Liên hệ CSKH qua website
### 6. Cơ sở dữ liệu
Đồ án sử dụng MongoDB, hệ thống quản lý cơ sở dữ liệu NoSQL, vì nhiều lý do:
  <ul>
      <li> Linh hoạt: MongoDB không yêu cầu lược đồ cố định như các hệ thống cơ sở dữ liệu SQL truyền thống. Cho phép ProjectX thay đổi cấu trúc dữ liệu khi cần thiết, tăng tốc độ phát triển và thích ứng với các yêu cầu thay đổi trong quá trình phát triển phần mềm.
      <li> Hiệu suất: MongoDB được thiết kế để xử lý lượng dữ liệu lớn và phức tạp, với khả năng mở rộng ngang và dọc. Đảm bảo ProjectX có thể xử lý lượng truy cập ngày càng tăng và lượng dữ liệu ngày càng lớn.
      <li> Hỗ trợ JSON: MongoDB sử dụng định dạng BSON (một biến thể của JSON) đễ lưu trữ dữ liệu, giúp việc tương tác với dữ liệu trở nên dễ dàng hơn, đặc biệt khi làm việc với các ứng dụng web và di động hiện đại. 
      </ul> 

| Thuộc tính        | Kiểu dữ liệu | Mô tả                                                             |
|------------------|--------------|-------------------------------------------------------------------|
| username         | string       | Tên tài khoản của người dùng                                      |
| email            | string       | Email người dùng                                                  |
| useraccountname  | string       | Tên người dùng                                                    |
| zaloapi          | string       | Key API cho phép người dùng sử dụng các tính năng của ZaloAI       |
| fptapi           | string       | Key API FPTAI cho phép người dùng sử dụng các tính năng của FPTAI |
| password         | string       | Mật khẩu                                                          |
| created_at       | date         | Ngày tạo tài khoản                                                |
| last_used_at     | date         | Ngày sử dụng gần nhất                                             |
| premium          | boolean      | Xác định gói đăng ký của người dùng                               |

### 7. Use Case Diagram
![use case diagram](https://github.com/TTMTN/Images/assets/127732884/a87c6c17-ffbb-43d6-94aa-49e4641745ac)
### 8. Người dùng
ProjectX hướng đến mọi người dùng, từ những người dùng cá nhân làm việc trong lĩnh vực công nghệ thông tin hoặc người không chuyên, đến những nhà phát triển ứng dụng có thể sử dụng các tính năng và công nghệ của ứng dụng trong việc phát triển các ứng dụng khác nhau hoặc các tổ chức doanh nghiệp có thể sử dụng ứng dụng để tự động hóa quy trình làm việc, cải thiện hiệu suất và tiết kiệm thời gian. Mục tiêu của ProjectX là giúp mọi người tận dụng tối đa công nghệ để cải thiện cuộc sống và công việc
### 9. Mục tiêu
   -	**Đáp ứng nhu cầu của người dùng**:
       <ul>
      <li>ProjectX sẽ được thiết kế để đáp ứng các yêu cầu cụ thể của người dùng, từ những người dùng cá nhân đến các nhà phát triển và tổ chức doanh nghiệp, đảm bảo tính ổn định và dễ sử dụng.i</li>
      </ul>
    	
   -	**Thay thế cho các ứng dụng cũ**:
      <ul>
      <li>Ứng dụng này nhằm mục đích thay thế các công cụ lỗi thời bằng cách cung cấp một giao diện người dùng cải tiến và các tính năng mới, giúp người dùng quản lý công việc một cách hiệu quả hơn.
      </ul>
    	
   -	**Tính năng tiêu chuẩn và mới**:
       <ul>
      <li>ProjectX sẽ tích hợp các tính năng tiêu chuẩn hiện có trên thị trường và phát triển thêm các tính năng mới để hỗ trợ tối đa cho người dùng và tự động hóa các quy trình làm việc.
      </ul>
    	
   -	**Giao diện thân thiện**:
      <ul>
      <li> Giao diện của ProjectX sẽ được thiết kế để thân thiện với người dùng, với bố cục hợp lý, hài hòa về màu sắc và mang tính đồng bộ cao.
      </ul>
    	
   -  **Tương thích với nhiều hệ điều hành**:
      <ul>
      <li>Ứng dụng sẽ tương thích với các hệ điều hành phổ biến như Windows Vista SP1, Windows 8.1, Windows 10, Windows 11 và có kế hoạch mở rộng tương thích với các nền tảng khác như MacOS và Linux trong tương lai.
      </ul>  
      
   -	**Hoạt động ổn định**:
       <ul>
      <li> ProjectX sẽ được thiết kế để hoạt động ổn định, tránh xung đột với hệ thống và giảm thiểu sự cố cho người dùng.
      </ul>
      
   -	**Dễ dàng mở rộng và nâng cấp**:
       <ul>
      <li> Ứng dụng sẽ được xây dựng để dễ dàng mở rộng và nâng cấp theo nhu cầu của người dùng.
      
   -	**Phân quyền cho người dùng**:
       <ul>
      <li> ProjectX sẽ có khả năng phân quyền cho người dùng thông qua tài khoản, giúp quản lý và kiểm soát quyền truy cập một cách hiệu quả.
      </ul>
    	
   -	**Trở thành lựa chọn hàng đầu của khách hàng**:
      <ul>
      <li> Mục tiêu cuối cùng của ProjectX là trở thành một trong những ứng dụng được người dùng lựa chọn và tin tưởng sử dụng.
      </ul> 

### 10.	Hệ thống third party / API
- **Module đọc văn bản thành giọng nói:**
  <ul>
      <li> Dự án sử dụng ffplay và ffmpeg, hai công cụ đa phương tiện mạnh mẽ, để phát và chuyển đổi các tệp âm thanh và video. Đặc biệt, ffmpeg được sử dụng để tải tệp M3U8 và lưu nó dưới dạng MP3.
    </ul>
- **API dịch ngôn ngữ:**
  <ul>
      <li> Dự án sử dụng API dịch ngôn ngữ từ Google để dịch văn bản từ một ngôn ngữ này sang ngôn ngữ khác. HttpClient được sử dụng để gửi yêu cầu GET đến URL của API dịch ngôn ngữ Google, và kết quả trả về là một chuỗi JSON chứa văn bản đã dịch.
    </ul>
- **API Gemini Pro:**
  <ul>
      <li> API này được sử dụng để tương tác với người dùng thông qua một giao diện chat đơn giản. Quá trình gửi thông tin và nhận phản hồi từ server tương tự như việc sử dụng API dịch ngôn ngữ.
    </ul>
- **A10.4	API đọc văn bản thành giọng nói:**
  <ul>
      <li> Đồ án sử dụng API đọc văn bản thành giọng nói (TTS) từ ZaloAI để chuyển đổi văn bản thành giọng nói tự nhiên. Module ffplay được sử dụng để phát văn bản qua thiết bị âm thanh trên máy người dùng.
(Text to Audio Converter, n.d.)Dự án sử dụng API từ FPTAI để so sánh khuôn mặt. Hai hình ảnh được người dùng tải lên được gửi đến API và kết quả phân tích được nhận về dưới dạng JSON. Kết quả sau đó được phân tích và hiển thị trên giao diện người dùng.
    </ul>
  Trong tất cả các quá trình này, HttpClient được sử dụng để gửi yêu cầu HTTP, MultipartFormDataContent để tạo nội dung yêu cầu, và JsonConvert để phân tích chuỗi JSON trả về từ API.

# III. Tác giả

| MSSV       | Họ và Tên          | Email                   | Github                                                                                                                      | Vai trò                |
| ---------- | ------------------ | ----------------------- | --------------------------------------------------------------------------------------------------------------------------- | -----------------------|
| `22521145` | Trương Lê Vĩnh Phúc - Trưởng nhóm| 222521145@gm.uit.edu.vn | [![](https://img.shields.io/badge/sloweyyy-%2324292f.svg?style=flat-square&logo=github      )](https://github.com/sloweyyy) |Leader, Front-end Developer, Back-end Developer, UX/UI Designer, Researcher|
| `22521644` | Trần Huỳnh Nhã Uyên| 22521644@gm.uit.edu.vn | [![](https://img.shields.io/badge/tranuyn-%2324292f.svg?style=flat-square&logo=github      )](https://github.com/tranuyn) | Researcher, Front-end Developer, UI/UX Designer|
| `22520936` | Trần Thị Mộng Trúc Ngân| 22520936@gm.uit.edu.vn | [![](https://img.shields.io/badge/TTMTN-%2324292f.svg?style=flat-square&logo=github      )](https://github.com/TTMTN) |Researcher, Back-end Developer |

# IV. Người hướng dẫn
Ths. Huỳnh Tuấn Anh       
# V. Tổng kết
### 1.	Ưu điểm
- Tích hợp nhiều công cụ và chức năng đa dạng, phục vụ cho nhiều người dùng và lĩnh vực khác nhau.
- Giao diện thân thiện, dễ thao tác và đăng ký, giúp người dùng tiếp cận và sử dụng ứng dụng một cách thuận tiện.
- Có khả năng mở rộng và nâng cấp, đáp ứng nhu cầu phát triển và thay đổi của người dùng.
- Website giới thiệu trực quan, cho phép người dùng liên hệ hỗ trợ khi cần.
### 2.	Nhược điểm
Tuy ProjectX mang lại nhiều lợi ích, nhưng cũng còn một số hạn chế cần khắc phục:
-	Chưa hỗ trợ đa nền tảng: Hiện tại, ứng dụng chưa thể sử dụng trên nhiều nền tảng khác nhau, hạn chế khả năng tiếp cận của người dùng.
-	Yêu cầu kết nối internet: Người dùng cần có kết nối internet để sử dụng ứng dụng, gây bất tiện trong nhiều trường hợp.
Sản phẩm là kết quả sau quá trình cùng nhau thực hiện đồ án của những thành viên trong nhóm. Thông qua quá trình này, các thành viên đã có cho mình những lượng kiến thức và kỹ năng chuyên môn nhất định về quy trình lập trình thực tế, hiểu hơn về lập trình và có riêng cho mình những bài học quý giá làm hành trang cho công việc sau này.

Ngoài ra, nhóm cũng muốn gửi lời cảm ơn chân thành và sự tri ân sâu sắc đến giảng viên giảng dạy, thầy Huỳnh Tuấn Anh đã cùng đồng hành với nhóm trong suốt quá trình thực hiện đồ án để có được thành quả như hôm nay.

Sản phẩm của nhóm có thể còn nhiều thiếu sót trong quá trình xây dựng và phát triển. Vì vậy, đừng ngần ngại gửi những đóng góp hoặc ý kiến của bạn. Mỗi đóng góp của các bạn đều sẽ được ghi nhận và sẽ là động lực để nhóm có thể hoàn thiện sản phẩm hơn nữa.

Cảm ơn bạn đã quan tâm!

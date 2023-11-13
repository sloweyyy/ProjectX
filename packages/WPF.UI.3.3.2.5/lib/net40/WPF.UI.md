

# WPF.UI

| 版本号 | 说明                                    |   修订时间 |
| :----: | :-------------------------------------- | ---------: |
| V3.1.0 | 初版                                    | 2023.05.15 |
| V3.2.0 | 增加Log记录功能，config.ini文件读写操作 | 2023.08.28 |
| V3.2.3 | 增加Access数据库读写类                  | 2023.09.14 |

## WPF.UI 使用教程

### 介绍

"WPF.UI" 包是一个用于构建 Windows Presentation Foundation (WPF) 用户界面的工具包。

实现WPF控件的属性扩展，使其支持更多的自定义属性，突破控件原有属性的限制。

将重复的样式和模板打包成通用的资源字典，大幅提高了模板化开发的便捷性，提高代码的可重用性。

### 安装教程

1. 在解决方案栏中的项目右键 -> 管理NuGet 程序包

![image-20230404110834614](https://raw.githubusercontent.com/IWPFI/Picture/main/WPF.UI202304130951231.png)

2. 在NuGet包管理器中找到WPF.UI并安装

![image-20230404111513885](https://raw.githubusercontent.com/IWPFI/Picture/main/WPF.UI202304130952800.png)

### 使用说明

#### XAML

1. **添加引用**：

```xaml
<!--名称可以自定义-->
xmlns:wpfui="clr-namespace:WPF.UI;assembly=WPF.UI" 
```

2. **可以在资源字典、样式以及控件中使用**

> **在样式中使用**

```xml
<Style ··· >
	<Setter Property="wpfui:WPFUI.CornerRadius" Value="10 0 0 10"/>
    <Setter Property="wpfui:WPFUI.MouseOverBackground" Value="#FFC1C1C1"/>
    <Setter Property="wpfui:WPFUI.MouseOverBorderBrush" Value="#FFC1C1C1"/>
    <Setter Property="wpfui:WPFUI.CheckedBackground" Value="#FF848484"/>
    ···
</Style>
```

> **在控件中使用**

```xaml
<Button wpfui:WPFUI.Tag="WPF.UI" 
        wpfui:WPFUI.Background="AntiqueWhite" 
        wpfui:WPFUI.Text="{Binding ...}"
        ···/>
```

> **在控件模板中使用：**
>
> > 在使用控件模板和触发器模板时，编译器缺乏代码智能提示功能。开发者需要手动编写代码，此处需要细心谨慎。

```xaml
<ControlTemplate TargetType="Button">
    <Border x:Name="border"
        Width="{TemplateBinding Width}"
        Height="{TemplateBinding Height}"
        Margin="{TemplateBinding Margin}"
        Background="{TemplateBinding Background}"
        BorderBrush="{TemplateBinding BorderBrush}"
        BorderThickness="{TemplateBinding wpfui:WPFUI.BorderThickness}"
        CornerRadius="10">
        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="50" />
                <ColumnDefinition Width="*" />
                <ColumnDefinition Width="50" />
            </Grid.ColumnDefinitions>
            <Border Background="{TemplateBinding wpfui:WPFUI.Background}" CornerRadius="{TemplateBinding wpfui:WPFUI.CornerRadius}" />
            <Label
                HorizontalAlignment="Center"
                VerticalAlignment="Center"
                Content="{TemplateBinding wpfui:WPFUI.Tag}"
                FontFamily="Webdings"
                FontSize="{TemplateBinding wpfui:WPFUI.FontSize}" />
            <ContentControl Grid.Column="1"
                HorizontalAlignment="Center"
                VerticalAlignment="Center"
                Content="{TemplateBinding Content}" />
            <Border Grid.Column="2"
                Background="{TemplateBinding wpfui:WPFUI.Background}"
                CornerRadius="{TemplateBinding wpfui:WPFUI.MouseOverCornerRadius}" />
        </Grid>
    </Border>
    <ControlTemplate.Triggers>
        <Trigger Property="IsMouseOver" Value="True">
            <Setter TargetName="border" 
                 Property="BorderBrush" 
                 Value="{Binding Path=(wpfui:WPFUI.MouseOverBorderBrush), RelativeSource={RelativeSource TemplatedParent}}" />
            <Setter TargetName="border" 
                 Property="Background" 
                 Value="{Binding Path=(wpfui:WPFUI.MouseOverBackground), RelativeSource={RelativeSource TemplatedParent}}" />
        </Trigger>
        <Trigger Property="IsPressed" Value="True">
            <Setter TargetName="border" 
                 Property="BorderBrush" 
                 Value="{Binding Path=(wpfui:WPFUI.CheckedBorderBrush), RelativeSource={RelativeSource TemplatedParent}}" />
            <Setter TargetName="border"
                 Property="Background"
                 Value="{Binding Path=(wpfui:WPFUI.CheckedBackground), RelativeSource={RelativeSource TemplatedParent}}" />
        </Trigger>
    </ControlTemplate.Triggers>
</ControlTemplate>

```

**注意：在触发器绑定语法会有所不同**

```xaml
<Trigger Property="IsMouse" Value="True">
    <Setter TargetName="border" 
            Property="BorderBrush" 
            Value="{Binding Path=(wpfui:WPFUI.MouseOverBorderBrush),RelativeSource={RelativeSource TemplatedParent}}"/>
    <Setter TargetName="border" 
            Property="Background" 
            Value="{Binding Path=(wpfui:WPFUI.MouseOverBackground),RelativeSource={RelativeSource TemplatedParent}}"/>
</Trigger>
```


---

> 完整示例：

```xaml
<Style TargetType="Button">
    <Setter Property="Height" Value="75" />
    <Setter Property="Width" Value="200" />
    <Setter Property="Tag" Value="10" />
    <Setter Property="FontSize" Value="24" />
    <Setter Property="wpfui:WPFUI.FontSize" Value="35" />
    <Setter Property="wpfui:WPFUI.Tag" Value="=" />
    <Setter Property="wpfui:WPFUI.CornerRadius" Value="10 0 0 10" />
    <Setter Property="wpfui:WPFUI.MouseOverCornerRadius" Value="0 10 10 0" />
    <Setter Property="wpfui:WPFUI.BorderThickness" Value="1" />
    <Setter Property="wpfui:WPFUI.Background" Value="Gray" />
    <Setter Property="wpfui:WPFUI.MouseOverBackground" Value="#FFC1C1C1" />
    <Setter Property="wpfui:WPFUI.MouseOverBorderBrush" Value="#FFC1C1C1" />
    <Setter Property="wpfui:WPFUI.CheckedBackground" Value="#FF848484" />
    <Setter Property="wpfui:WPFUI.CheckedBorderBrush" Value="#FF848484" />
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="Button">
                <Border x:Name="border"
                    Width="{TemplateBinding Width}"
                    Height="{TemplateBinding Height}"
                    Margin="{TemplateBinding Margin}"
                    Background="{TemplateBinding Background}"
                    BorderBrush="{TemplateBinding BorderBrush}"
                    BorderThickness="{TemplateBinding wpfui:WPFUI.BorderThickness}"
                    CornerRadius="10">
                    <Grid>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="50" />
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="50" />
                        </Grid.ColumnDefinitions>
                        <Border Background="{TemplateBinding wpfui:WPFUI.Background}" 
                                CornerRadius="{TemplateBinding wpfui:WPFUI.CornerRadius}" />
                        <Label
                            HorizontalAlignment="Center"
                            VerticalAlignment="Center"
                            Content="{TemplateBinding wpfui:WPFUI.Tag}"
                            FontFamily="Webdings"
                            FontSize="{TemplateBinding wpfui:WPFUI.FontSize}" />
                        <ContentControl Grid.Column="1"
                            HorizontalAlignment="Center"
                            VerticalAlignment="Center"
                            Content="{TemplateBinding Content}" />
                        <Border Grid.Column="2"
                            Background="{TemplateBinding wpfui:WPFUI.Background}"
                            CornerRadius="{TemplateBinding wpfui:WPFUI.MouseOverCornerRadius}" />
                    </Grid>
                </Border>
                <ControlTemplate.Triggers>
                    <Trigger Property="IsMouseOver" Value="True">
                        <Setter TargetName="border" Property="BorderBrush" Value="{Binding Path=(wpfui:WPFUI.MouseOverBorderBrush), RelativeSource={RelativeSource TemplatedParent}}" />
                        <Setter TargetName="border" Property="Background" Value="{Binding Path=(wpfui:WPFUI.MouseOverBackground), RelativeSource={RelativeSource TemplatedParent}}" />
                    </Trigger>
                    <Trigger Property="IsPressed" Value="True">
                        <Setter TargetName="border" Property="BorderBrush" Value="{Binding Path=(wpfui:WPFUI.CheckedBorderBrush), RelativeSource={RelativeSource TemplatedParent}}" />
                        <Setter TargetName="border" Property="Background" Value="{Binding Path=(wpfui:WPFUI.CheckedBackground), RelativeSource={RelativeSource TemplatedParent}}" />
                    </Trigger>
                </ControlTemplate.Triggers>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```



------

#### LogNet类 「日志操作类」

**1. WriteDebug(string content, string fileName = null, string path = null)**

   - 功能：记录调试级别的日志信息。
   - 参数：
     - content：要记录的日志内容。
     - fileName（可选）：日志文件名。
     - path（可选）：日志文件路径。
   - 用法示例：
     ```csharp
     LogNet.WriteDebug("Debug log message");
     LogNet.WriteDebug("Debug log message", "debug.log");
     LogNet.WriteDebug("Debug log message", "debug.log", "C:\");
     ```

**2. WriteInfo(string content, string fileName = null, string path = null)**

   - 功能：记录普通信息级别的日志。
   - 参数：
     - content：要记录的日志内容。
     - fileName（可选）：日志文件名。
     - path（可选）：日志文件路径。
   - 用法示例：
     ```csharp
     LogNet.WriteInfo("Info log message");
     LogNet.WriteInfo("Info log message", "info.log");
     LogNet.WriteInfo("Info log message", "info.log", "C:\");
     ```

**3. WriteError(string content, string fileName = null, string path = null)**

   - 功能：记录错误级别的日志。
   - 参数：
     - content：要记录的日志内容。
     - fileName（可选）：日志文件名。
     - path（可选）：日志文件路径。
   - 用法示例：
     ```csharp
     LogNet.WriteError("Error log message");
     LogNet.WriteError("Error log message", "error.log");
     LogNet.WriteError("Error log message", "error.log", "C:\");
     ```

**4. WriteNewLine(string fileName = null, string path = null)**

   - 功能：在日志中插入空行。
   - 参数：
     - fileName（可选）：日志文件名。
     - path（可选）：日志文件路径。
   - 用法示例：
     ```csharp
     LogNet.WriteNewLine();
     LogNet.WriteNewLine("log.txt");
     LogNet.WriteNewLine("log.txt", "C:\");
     ```

**5. WriteString(string content, string fileName = null, string path = null)**

   - 功能：直接写入一段字符串到日志。
   - 参数：
     - content：要写入的字符串。
     - fileName（可选）：日志文件名。
     - path（可选）：日志文件路径。
   - 用法示例：
     ```csharp
     LogNet.WriteString("Custom log message");
     LogNet.WriteString("Custom log message", "custom.log");
     LogNet.WriteString("Custom log message", "custom.log", "C:\");
     ```

**6. WriteTitle(string content, string fileName = null, string path = null)**

   - 功能：写入标题形式的日志，用于标识具体操作或事件。
   - 参数：
     - content：标题内容。
     - fileName（可选）：日志文件名。
     - path（可选）：日志文件路径。
   - 用法示例：
     ```csharp
     LogNet.WriteTitle("Log Title");
     LogNet.WriteTitle("Log Title", "log.txt");
     LogNet.WriteTitle("Log Title", "log.txt", "C:\");
     ```

**7. WriteWarn(string content, string fileName = null, string path = null)**

   - 功能：记录警告级别的日志。
   - 参数：
     - content：要记录的日志内容。
     - fileName（可选）：日志文件名。
     - path（可选）：日志文件路径。
   - 用法示例：
     ```csharp
     LogNet.WriteWarn("Warning log message");
     LogNet.WriteWarn("Warning log message", "warning.log");
     LogNet.WriteWarn("Warning log message", "warning.log", "C:\");
     ```

**8. WriteFatal(string content, string fileName = null, string path = null)**
   - 功能：记录致命错误级别的日志。
   - 参数：
     - content：要记录的日志内容。
     - fileName（可选）：日志文件名。
     - path（可选）：日志文件路径。
   - 用法示例：
     ```csharp
     LogNet.WriteFatal("Fatal log message");
     LogNet.WriteFatal("Fatal log message", "fatal.log");
     LogNet.WriteFatal("Fatal log message", "fatal.log", "C:\");
     ```



------



#### FileIO类 

FileIO类提供了对文件系统的操作方法。下面是各个方法的说明：

**1. ClearMemory()**

   - 功能：清除内存中的缓存数据。
   - 用法示例：
```csharp
FileIO.ClearMemory();
```

**2. ClearMemoryAsncy()**
   - 功能：异步清除内存中的缓存数据。
   - 用法示例：
```csharp
await FileIO.ClearMemoryAsncy();
```

**3. AddBlankString(int length)**
   - 功能：在当前位置添加指定长度的空白字符串。
   - 参数：length - 空白字符串的长度。
   - 返回值：返回添加空白字符串后的结果。
   - 用法示例：
```csharp
string result = FileIO.AddBlankString(5);
```

**4. Encode(string data)**
   - 功能：对给定的字符串进行编码。
   - 参数：data - 要编码的字符串。
   - 返回值：返回编码后的字符串。
   - 用法示例：
```csharp
string encodedData = FileIO.Encode("Hello, World!");
```

**5. PingIP(string ip)**
   - 功能：检查给定的IP地址是否可通。
   - 参数：ip - 要检查的IP地址。
   - 返回值：如果IP地址可通，则返回true；否则返回false。
   - 用法示例：
```csharp
bool isReachable = FileIO.PingIP("192.168.0.1");
```

**6. Decode(string data)**
   - 功能：对给定的字符串进行解码。
   - 参数：data - 要解码的字符串。
   - 返回值：返回解码后的字符串。
   - 用法示例：
```csharp
string decodedData = FileIO.Decode("SGVsbG8sIFdvcmxkIQ==");
```

**7. SetIniPath(string striniFilePath)**
   - 功能：设置INI文件的路径。
   - 参数：striniFilePath - INI文件的路径。
   - 用法示例：
```csharp
FileIO.SetIniPath("/path/to/ini/file.ini");
```

**8. ReadIniString(string section, string key)**
   - 功能：从指定的INI文件中读取指定section和key的值。
   - 参数：section - INI文件中的section名称。
   -       key - INI文件中的key名称。
   - 返回值：返回读取到的值。
   - 用法示例：
```csharp
string value = FileIO.ReadIniString("SectionName", "KeyName");
```

**9. ReadIniInt(string section, string key)**
   - 功能：从指定的INI文件中读取指定section和key的整数值。
   - 参数：section - INI文件中的section名称。
   -       key - INI文件中的key名称。
   - 返回值：返回读取到的整数值。
   - 用法示例：
```csharp
int intValue = FileIO.ReadIniInt("SectionName", "KeyName");
```

**10. ReadIniDouble(string section, string key)**

- 功能：从指定的INI文件中读取指定section和key的双精度浮点数值。
- 参数：
  - section - INI文件中的section名称。
  - key - INI文件中的key名称。
- 返回值：返回读取到的双精度浮点数值。
- 用法示例：

```csharp
double doubleValue = FileIO.ReadIniDouble("SectionName", "KeyName");
```

**11. Replace(string str, string oldStr, string newStr)**

- 功能：在给定的字符串中替换所有的旧字符串为新字符串。

- 参数：str - 原始字符串。
  - oldStr - 要替换的旧字符串。
  - newStr - 替换成的新字符串。
- 返回值：返回替换后的字符串。
- 用法示例：

```csharp
string replacedString = FileIO.Replace("Hello, World!", "World", "Everyone");
```

**12. WriteIniString(string section, string key, string val)**

- 功能：向指定的INI文件中写入指定section和key的值。
- 参数：section - INI文件中的section名称。
  - key - INI文件中的key名称。
  - val - 要写入的值。
- 用法示例：

```csharp
FileIO.WriteIniString("SectionName", "KeyName", "Value");
```

**13. DeleteSection(string Section)**

- 功能：删除指定的INI文件中的section。
- 参数：Section - 要删除的section名称。
- 用法示例：

```csharp
FileIO.DeleteSection("SectionName");
```

**14. DeleteKey(string section, string key)**

- 功能：删除指定的INI文件中的key。
- 参数：
  - section - INI文件中的section名称。
  -  key - INI文件中的key名称。
- 用法示例：

```csharp
FileIO.DeleteKey("SectionName", "KeyName");
```

**15. GetSectionAllKeys(string Key)**

- 功能：获取指定INI文件中的所有key。
- 参数：Key - 要获取的key的前缀。
- 返回值：返回匹配到的所有key列表。
- 用法示例：

```csharp
List<string> keys = FileIO.GetSectionAllKeys("SectionName");
```

**16. ConnTcp(string IPaddress, int Port, int WaitTime = 3)**
- 功能：建立TCP连接。
- 参数：
    - IPaddress - 目标IP地址。
    - Port - 目标端口号。
    - WaitTime - 连接超时时间（单位：秒）。
    - 返回值：返回建立的TcpClient对象。
- 用法示例：

```csharp
TcpClient client = FileIO.ConnTcp("192.168.0.1", 8080, 5);
```

**17. FileWrite(string path, string str, bool append = false, string encoding = "utf-8")**

- 功能：将字符串写入文件。
- 参数：path - 文件路径。
  - str - 要写入的字符串。
- append - 是否追加模式写入（默认为覆盖模式）。
- encoding - 文件编码（默认为UTF-8）。
- 用法示例：

```csharp
FileIO.FileWrite("path/to/file.txt", "Hello, World!", true);
```

**18. GetSectionAllKeys(string Key)**

- 功能：获取指定目录中的所有文件。
- 参数：Key - 要获取的文件名的前缀。
- 返回值：返回匹配到的所有文件列表。
- 用法示例：

```csharp
List<string> files = FileIO.GetFiles("path/to/directory", "*.*");
```

**19. FileRead(string path, string encoding = "utf-8")**

- 功能：读取文件内容。
- 参数：
  - path - 文件路径。
  - encoding - 文件编码（默认为UTF-8）。
- 返回值：返回文件的内容。
- 用法示例：

```csharp
string content = FileIO.FileRead("path/to/file.txt");
```

**20. FileCopy(string sourceFileName, string destFileName, bool overwrite = true)**

- 功能：复制文件。
- 参数：
  - sourceFileName - 源文件路径。
  - destFileName - 目标文件路径。
  - overwrite - 是否覆盖已存在的文件（默认为覆盖模式）。
- 用法示例：

```csharp
FileIO.FileCopy("path/to/source/file.txt", "path/to/destination/file.txt");
```

**21. CreateFolder(string fileName = null, string filePath = null)**

- 功能：获取保存文件夹的路径。
- 参数：
  - `fileName `（可选）：保存文件夹名称，默认为空字符串。
  - `filePath  `（可选）：保存文件夹的根路径，默认为空字符串。
- 返回值：返回保存文件夹的路径。
- 用法示例：

```csharp
CreateFolder("文件夹","D:\\")
```

**22.FileToBytes(string path)**

   - 功能：将指定文件转换为字节数组。
   - 参数：
     - `path`：要转换的文件的路径。
   - 返回值：返回转换后的字节数组。

**23BytesToFile(byte[] bytes, string saveFile)**

   - 功能：将字节数组保存为文件。
   - 参数：
     - `bytes`：要保存的字节数组。
     - `saveFile`：保存文件的路径。
   - 返回值：无返回值。

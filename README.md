## 项目简介 | Overview

旧版 Shmups GameBuilder（STGBuilder）项目文件的解包与还原工具。  
This is a Windows Forms utility that extracts and reconstructs legacy Shmups GameBuilder project assets from `.gamedat` archives.

## 功能 | Features

- 选择并解包 `.gamedat` 资源包，自动输出到同级目录的 `UnpackData` 文件夹  
  Select a `.gamedat` archive and unpack its contents into an `UnpackData` folder beside the source file.
- 读取打包索引与伴随的 `*.sbd` 字符串表，恢复原始文件名与路径（Shift_JIS/CP932 编码）  
  Reads the embedded index and companion `*.sbd` string table to restore original file names and paths (Shift_JIS/CP932 encoding).
- 进度条与状态提示，避免界面卡死  
  Progress bar and status label to keep the UI responsive during extraction.

## 环境要求 | Requirements

- Windows 10/11
- .NET Framework 4.8（项目目标框架）  
  .NET Framework 4.8 (target framework)
- Visual Studio 2019+（如需重新编译）  
  Visual Studio 2019+ if you want to rebuild.

## 快速开始 | Quick Start

1. 编译或直接运行已编译的 `STGBUNPACK_SHARP.exe`。  
   Build or run the packaged `STGBUNPACK_SHARP.exe`.
2. 点击 “选择.gamedat文件” 按钮，指向需要解包的 `.gamedat`。  
   Click “选择.gamedat文件” to pick the target `.gamedat` archive.
3. 点击 “解包”，等待进度条完成。所有资源会写入源文件同目录下的 `UnpackData\`。  
   Press “解包” to start. Extracted files appear under `UnpackData\` next to the archive.

> 文件名解析依赖随包的 `.sbd` 数据，使用 Shift_JIS（CP932）编码。若文件名出现乱码，请确认原始包是否包含对应字符串表。  
> File names rely on the embedded `.sbd` table (Shift_JIS/CP932). If you see garbled names, the archive may lack proper string data.

## 构建指南 | Build From Source

1. 使用 Visual Studio 打开 `STGBUNPACK_SHARP.sln`。  
   Open `STGBUNPACK_SHARP.sln` in Visual Studio.
2. 确认目标框架为 .NET Framework 4.8，选择 Debug 或 Release 配置。  
   Keep target framework at .NET Framework 4.8 and choose Debug/Release.
3. 直接生成解决方案即可获得可执行文件（输出路径在 `bin\Debug\` 或 `bin\Release\`）。  
   Build the solution; executables appear in `bin\Debug\` or `bin\Release\`.

## 工作原理简述 | How It Works

- 跳过头部后读取文件数量，遍历索引提取每个文件的长度、偏移与名称占位。  
  After the header, the tool reads the file count and iterates index entries to get size, offset, and placeholder names.
- 使用首个索引指向的 `.sbd` 字符串块，正则匹配恢复真实文件路径与扩展名。  
  The first index entry points to the `.sbd` string block; regex matching restores full file paths and extensions.
- 将每个文件写入 `UnpackData\`，在需要时自动创建目录。  
  Each file is written to `UnpackData\`, creating directories as needed.

## 目录结构 | Project Layout

- `Form1.cs`：解包逻辑与事件处理  
  `Form1.cs`: unpack logic and UI events.
- `Form1.Designer.cs` / `Form1.resx`：窗体布局与资源  
  `Form1.Designer.cs` / `Form1.resx`: form layout and resources.
- `Program.cs`：程序入口  
  `Program.cs`: application entry point.
- `STGBUNPACK_SHARP.csproj`：项目配置（.NET Framework 4.8）  
  `STGBUNPACK_SHARP.csproj`: project configuration (.NET Framework 4.8).

## 已知限制 | Known Limitations

- 仅测试过旧版 Shmups GameBuilder 打包格式；对其他版本或自定义包未做兼容性保证。  
  Only validated against legacy Shmups GameBuilder archives; other variants may not work.
- 文件名依赖 `.sbd` 字符串表，缺失时将回退为占位名。  
  File names fall back to placeholders if the `.sbd` table is missing.

## 许可协议 | License

本项目采用 MIT License，详见 `LICENSE`。  
MIT License, see `LICENSE`.


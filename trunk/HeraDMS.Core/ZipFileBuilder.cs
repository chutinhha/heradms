using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.IO;

namespace HeraDMS.Core
{
    /// <summary>
    /// 允许你添加文件和文件夹，最终输出到zip文件。因为它要处理文件流，所以该类实现了IDisposable接口。
    /// </summary>
    public class ZipFileBuilder : IDisposable
    {
        private bool disposed = false;  
 
        ZipOutputStream zipStream = null;  
        protected ZipOutputStream ZipStream  
        {  
            get { return zipStream; }  
 
        }  
 
        ZipEntryFactory factory = null;  
        private ZipEntryFactory Factory  
        {  
            get { return factory; }  
        }  
 
 
        public ZipFileBuilder(Stream outStream)  
        {  
            zipStream = new ZipOutputStream(outStream);  
            zipStream.SetLevel(9); //best compression  
 
            factory = new ZipEntryFactory(DateTime.Now);  
        }  
 
        /// <summary>
        /// 创建一个压缩文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        public void Add(string fileName, Stream fileStream)  
        {  
            //create a new zip entry              
            ZipEntry entry = factory.MakeFileEntry(fileName);  
            entry.DateTime = DateTime.Now;  
            ZipStream.PutNextEntry(entry);  
 
            byte[] buffer = new byte[65536];  
 
            int sourceBytes;  
            do 
            {  
                sourceBytes = fileStream.Read(buffer, 0, buffer.Length);  
                ZipStream.Write(buffer, 0, sourceBytes);  
            }  
            while (sourceBytes > 0);  
        }  
 
        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryName"></param>
        public void AddDirectory(string directoryName)  
        {  
            ZipEntry entry = factory.MakeDirectoryEntry(directoryName);  
            ZipStream.PutNextEntry(entry);  
        }  
 
        /// <summary>
        /// 压缩完成
        /// </summary>
        public void Finish()  
        {  
            if (!ZipStream.IsFinished)  
            {  
                ZipStream.Finish();  
            }  
        }  
 
        public void Close()  
        {  
            Dispose(true);  
            GC.SuppressFinalize(this);  
        }  
 
        public void Dispose()  
        {  
            this.Close();  
        }  
 
        protected virtual void Dispose(bool disposing)  
        {  
            if (!disposed)  
            {  
                if (disposing)  
                {  
                    if (ZipStream != null)  
                        ZipStream.Dispose();  
                }  
            }  
 
            disposed = true;  
        }  
    } 
}

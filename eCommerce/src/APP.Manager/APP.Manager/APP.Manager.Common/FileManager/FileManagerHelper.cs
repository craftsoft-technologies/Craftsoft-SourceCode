using System;
using System.IO;

namespace APP.Manager.Common.FileManager
{
    public static class FileManagerHelper
    {

        public static string MoveFile(string fileName, string srcFileDirectory, string desFileDirectory)
        {
            string srcRootPath = AppConfiguration.Instance.fileSetting.sourceRootPath;
            string desRootPath = AppConfiguration.Instance.fileSetting.destinationRootPath;

            CreateDirectoryIfNotExist(Path.Combine(desRootPath, desFileDirectory));

            string desFileName = ConstructNameIfDuplicateFileName(fileName, Path.Combine(desFileDirectory, fileName));

            File.Move(Path.Combine(srcRootPath, srcFileDirectory, fileName), Path.Combine(desRootPath, desFileDirectory, desFileName));

            return desFileName;

        }

        //Obsolete
        public static string CreateFile(string fileName, byte[] fileBytes, string desFileDirectory)
        {
            string desRootPath = AppConfiguration.Instance.fileSetting.destinationRootPath;

            CreateDirectoryIfNotExist(desRootPath + desFileDirectory);

            fileName = ConstructNameIfDuplicateFileName(fileName, desFileDirectory + fileName);

            using (FileStream fsNew = new FileStream(desRootPath + desFileDirectory + fileName, FileMode.Create, FileAccess.Write))
            {
                fsNew.Write(fileBytes, 0, fileBytes.Length);
            }

            return fileName;

        }


        public static byte[] GetFile(string desFilePath)
        {
            string desRootPath = AppConfiguration.Instance.fileSetting.destinationRootPath;

            using (FileStream fsSource = new FileStream(desRootPath + desFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the source file into a byte array.
                byte[] bytes = new byte[fsSource.Length];
                int numBytesToRead = (int)fsSource.Length;
                int numBytesRead = 0;
                //while (numBytesToRead > 0)
                //{
                // Read may return anything from 0 to numBytesToRead.
                int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                //    if (n == 0)
                //        break;

                //    numBytesRead += n;
                //    numBytesToRead -= n;
                //}

                return bytes;
            }

        }

        public static void ArchiveFile(string fileName, string desFileDirectory)
        {
            string archRootPath = AppConfiguration.Instance.fileSetting.archiveRootPath;
            string desRootPath = AppConfiguration.Instance.fileSetting.destinationRootPath;

            CreateDirectoryIfNotExist(Path.Combine(archRootPath, desFileDirectory));

            File.Move(Path.Combine(desRootPath, desFileDirectory, fileName), Path.Combine(archRootPath, desFileDirectory, fileName));

        }

        #region Helper Method

        public static void CreateDirectoryIfNotExist(string desDirectoryPath)
        {
            if (!IsDirectoryExists(desDirectoryPath))
                Directory.CreateDirectory(desDirectoryPath);
        }

        public static string ConstructNameIfDuplicateFileName(string fileName, string desFilePath)
        {
            if (IsFileExists(desFilePath))
                return string.Format("{0}_{1}", DateTime.Now.Ticks, fileName);
            else
                return fileName;
        }

        public static bool IsDirectoryExists(string desDirectoryPath)
        {
            return Directory.Exists(desDirectoryPath);
        }

        public static bool IsFileExists(string desFilePath)
        {
            string desRootPath = AppConfiguration.Instance.fileSetting.destinationRootPath;
            return File.Exists(Path.Combine(desRootPath + desFilePath));
        }

        public static void DeleteFile(string fileName, string desFileDirectory)
        {
            string desRootPath = AppConfiguration.Instance.fileSetting.destinationRootPath;

            ArchiveFile(fileName, desFileDirectory);
            File.Delete(Path.Combine(desRootPath, desFileDirectory, fileName));
        }
        #endregion

    }
}

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.AccessControl;
using System.Text;

namespace MemMap
{
    class Program
    {
        public void Main(string[] args)
        {
            MemFileExample();
            MemShareExample();

        }

        public void MemFileExample() 
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(@"C:\Files\data2.dat", FileMode.CreateNew, "map1", 1000))

            using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
            {
                Console.WriteLine($"Write 87 to data1.dat file starting at position 0.");
                accessor.Write(0, (byte)87);
                Console.ReadLine();

                Console.WriteLine($"Read from data.dat file starting at position 0:  {accessor.ReadByte(0)}");
                Console.ReadLine();

                byte[] data = Encoding.UTF8.GetBytes("test data");
                Console.WriteLine($"Convert string /'test data/' to byte. This is {data.Length} bytes");
                Console.ReadLine();

                accessor.WriteArray(1, data, 0, data.Length);
                Console.WriteLine($"Write {data} to data1.dat file starting at position 1.");
                Console.ReadLine();

                byte[] data1 = Encoding.UTF8.GetBytes("blob data");
                accessor.ReadArray(1, data1, 0, data.Length);

                Console.WriteLine($"Read from data.dat file starting at position 1:  {System.Text.Encoding.UTF8.GetString(data1)}");
                Console.ReadLine();
            }
            Console.ReadLine();
        }

        private void MemShareExample()
        {
            var memoryMappedFileSecurity = new MemoryMappedFileSecurity();
            memoryMappedFileSecurity.AddAccessRule(new AccessRule<MemoryMappedFileRights>("Everyone", MemoryMappedFileRights.Read, AccessControlType.Allow));

            MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen    //creates or opens a memory-mapped file that has a specific capacity in system memory.
                (@"Global\{0}",                                     //(string mapName, 
                1000,                                               //long capacity,
                MemoryMappedFileAccess.ReadWriteExecute,            //MemoryMappedFileAccess access,
                MemoryMappedFileOptions.None,                       //MemoryMappedFileOptions options,
                memoryMappedFileSecurity,                           //MemoryMappedFileSecurity memoryMappedFileSecurity,
                HandleInheritability.None);                         //HandleInheritability inheritability)

        }

    }
}

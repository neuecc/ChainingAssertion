using System;
using System.Linq;
using System.IO;

class Program
{
    public static void Main(string[] args)
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var srcDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\ChainingAssertion"));
        var srcName = "ChainingAssertion.MSTest.cs";
        var srcPath = Path.Combine(srcDir, srcName);
        var distDirFX40 = Path.Combine(srcDir, @"dist\net40");
        var distDirFX45 = Path.Combine(srcDir, @"dist\net45");
        var distPathFX40 = Path.Combine(distDirFX40, srcName);
        var distPathFX45 = Path.Combine(distDirFX45, srcName);

        if (!Directory.Exists(distDirFX40)) Directory.CreateDirectory(distDirFX40);
        if (!Directory.Exists(distDirFX45)) Directory.CreateDirectory(distDirFX45);

        var srcContents = File.ReadAllLines(srcPath);

        var inFX45 = false;
        var srcFX40 = srcContents.Where(src =>
        {
            if (src == "#if _CHAININGASSERTION_FX45") inFX45 = true;
            if (src == "#endif // _CHAININGASSERTION_FX45") { inFX45 = false; return false; }
            return !inFX45;
        }).ToArray();
        File.WriteAllLines(distPathFX40, srcFX40);

        var srcFX45 = srcContents.Where(src =>
        {
            if (src == "#if _CHAININGASSERTION_FX45") return false;
            if (src == "#endif // _CHAININGASSERTION_FX45") return false;
            return true;
        }).ToArray();
        File.WriteAllLines(distPathFX45, srcFX45);
    }
}
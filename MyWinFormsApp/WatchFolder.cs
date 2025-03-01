using System;
using System.IO;

using Guru.ExtensionMethod;
using Guru.DependencyInjection;
using Guru.Monitor.Abstractions;

namespace MyWinFormsApp
{
    public class WatchFolder : IDisposable
    {
        private readonly IFileSystemMonitor _FileSystemMonitor;

        private readonly IFileSystemHelper _FileSystemHelper;

        private readonly IContext _Context;

        public WatchFolder(string path, WatchFolder parent)
        {
            Path = path;
            Parent = parent;

            _FileSystemMonitor = DependencyContainer.Resolve<IFileSystemMonitor>();
            _FileSystemHelper = DependencyContainer.Resolve<IFileSystemHelper>();
            _Context = DependencyContainer.Resolve<IContext>();

            _FileSystemMonitor.Add(this, (_Context.Source + "/" + Path).FullPath(), Changed, Created, Deleted, Renamed);

            var directoryInfo = new DirectoryInfo((_Context.Source + "/" + Path).FullPath());
            foreach (var folder in directoryInfo.GetDirectories())
            {
                Children = Children.Append(new WatchFolder(Path + "/" + folder.Name, this));
            }
        }

        public string Path { get; set; }

        public WatchFolder Parent { get; set; }

        public WatchFolder[] Children { get; set; }

        private void Changed(string path)
        {
            if (path.IsFile())
            {
                Console.WriteLine($"file changed: {path}");
                foreach (var action in CommonActions)
                {
                    action.Invoke($"文件被修改: {path}");
                }
                foreach (var action in changeActions)
                {
                    action.Invoke(path);
                }
                _FileSystemHelper.WriteFile(Path + "/" + path.Name());
            }
        }

        private void Created(string path)
        {
            if (path.IsFile())
            {
                Console.WriteLine($"file created: {path}");
                foreach (var action in CommonActions)
                {
                    action.Invoke($"文件被创建: {path}");
                }
                foreach (var action in createdActions)
                {
                    action.Invoke(path);
                }
                _FileSystemHelper.WriteFile(Path + "/" + path.Name());
            }
            else if (path.IsFolder())
            {
                Console.WriteLine($"folder created: {path}");
                foreach (var action in CommonActions)
                {
                    action.Invoke($"文件夹被创建: {path}");
                }
                foreach (var action in createdFloderActions)
                {
                    action.Invoke(path);
                }
                _FileSystemHelper.CreateFolder(Path + "/" + path.Name());

                Children = Children.Append(new WatchFolder(Path + "/" + path.Name(), this));
            }
        }

        private void Deleted(string path)
        {
            Console.WriteLine($"deleted: {path}");
            foreach (var action in CommonActions)
            {
                action.Invoke($"文件被删除: {path}");
            }
            foreach (var action in deleteActions)
            {
                action.Invoke(path);
            }
            var folder = Children.FirstOrDefault(x => x.Path.EqualsIgnoreCase(Path + "/" + path.Name()));
            if (folder != null)
            {
                folder.Dispose();
                Children = Children.Remove(x => x == folder);
            }

            _FileSystemHelper.Delete(Path + "/" + path.Name());
        }

        private void Renamed(string oldPath, string newPath)
        {
            Console.WriteLine($"deleted: {oldPath}");
            foreach (var action in CommonActions)
            {
                action.Invoke($"文件被删除: {oldPath}");
            }
            foreach (var action in deleteActions)
            {
                action.Invoke(oldPath);
            }
            var folder = Children.FirstOrDefault(x => x.Path.EqualsIgnoreCase(Path + "/" + oldPath.Name()));
            if (folder != null)
            {
                folder.Dispose();
                Children = Children.Remove(x => x == folder);
            }

            _FileSystemHelper.Delete(Path + "/" + oldPath.Name());

            if (newPath.IsFile())
            {
                Console.WriteLine($"file created: {newPath}");
                foreach (var action in CommonActions)
                {
                    action.Invoke($"文件被创建: {oldPath}");
                }
                foreach (var action in createdActions)
                {
                    action.Invoke(oldPath);
                }
                foreach (var action in createdActions)
                {
                    action.Invoke(oldPath);
                }
                _FileSystemHelper.WriteFile(Path + "/" + newPath.Name());
            }
            else if (newPath.IsFolder())
            {
                Console.WriteLine($"folder created: {newPath}");
                foreach (var action in CommonActions)
                {
                    action.Invoke($"文件夹被创建: {oldPath}");
                }
                foreach (var action in createdFloderActions)
                {
                    action.Invoke(oldPath);
                }
                _FileSystemHelper.CreateFolder(Path + "/" + newPath.Name());

                Children = Children.Append(new WatchFolder(Path + "/" + newPath.Name(), this));
            }
        }
        protected IList<Action<string>> CommonActions = new List<Action<string>>();
        public void AddCommonAction(Action<string> func)
        {
            CommonActions.Add(func);

        }
        private IList<Action<string>> createdFloderActions = new List<Action<string>>();
        public void AddCreatedFloderAction(Action<string> func)
        {
            createdFloderActions.Add(func);

        }
        private IList<Action<string>> createdActions = new List<Action<string>>();
        public void AddCreatedAction(Action<string> func)
        {
            createdActions.Add(func);

        }
        private IList<Action<string>> deleteActions = new List<Action<string>>();
        public void AddDeleteAction(Action<string> func)
        {
            deleteActions.Add(func);

        }
       

        private IList<Action<string>> changeActions = new List<Action<string>>();
        public void AddChangeAction(Action<string> func)
        {
            changeActions.Add(func);

        }
        public void Dispose()
        {
            Children.Each(x => x.Dispose());

            _FileSystemMonitor.Remove(this, (_Context.Source + "/" + Path).FullPath(), Changed, Created, Deleted, Renamed);
        }
    }
}
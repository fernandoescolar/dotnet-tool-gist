namespace DotnetGist.Msbuild;

public class Project
{
    private const string ProjectName = "Project";
    private const string SdkAttributeName = "Sdk";
    private const string GistReferenceContainerName = "ProjectExtensions";
    private const string GistReferenceName = "GistReference";
    private readonly string _filepath;

    private readonly Lazy<XDocument> _document;

    public Project(string filepath)
    {
        _filepath = filepath;
        _document = new Lazy<XDocument>(() => XDocument.Load(filepath));
    }

    public string FilePath => _filepath;

    public bool IsSdkProject
        => _document.Value.Root?.Name.LocalName == ProjectName && _document.Value.Root?.Attribute(SdkAttributeName) != null;

    public List<GistReferenceItem> GetGistReferences()
    {
        var items = new List<GistReferenceItem>();
        if (IsSdkProject)
        {
            var ns = _document.Value.Root?.Name.Namespace ?? string.Empty;
            var itemGroups = _document.Value.Root?.Elements(ns + GistReferenceContainerName);
            if (itemGroups is not null)
            {
                foreach (var itemGroup in itemGroups)
                {
                    var elements = itemGroup.Elements(ns + GistReferenceName);
                    foreach (var element in elements)
                    {
                        var item = new GistReferenceItem();
                        item.Id = element.Attribute(nameof(GistReferenceItem.Id))?.Value;
                        item.Version = element.Attribute(nameof(GistReferenceItem.Version))?.Value;
                        item.FilePattern = element.Attribute(nameof(GistReferenceItem.FilePattern))?.Value;
                        item.OutputPath = element.Attribute(nameof(GistReferenceItem.OutputPath))?.Value;
                        items.Add(item);
                    }
                }
            }
        }

        return items;
    }

    public void AddGistReference(GistReferenceItem item)
    {
        if (IsSdkProject)
        {
            var itemGroup = default(XElement);
            var ns = _document.Value.Root?.Name.Namespace ?? string.Empty;
            var itemGroups = _document.Value.Root?.Elements(ns + GistReferenceContainerName);
            if (itemGroups is not null)
            {
                foreach(var i in itemGroups)
                {
                    var elements = i.Elements(ns + GistReferenceName);
                    if (elements.Any())
                    {
                        itemGroup = i;
                        break;
                    }
                }
            }

            if (itemGroup is null)
            {
                itemGroup = new XElement(ns + GistReferenceContainerName);
                _document.Value.Root?.Add(itemGroup);
            }

            var element = new XElement(ns + GistReferenceName);
            if (item.Id is not null)
            {
                element.Add(new XAttribute(nameof(GistReferenceItem.Id), item.Id));
            }

            if (item.Version is not null)
            {
                element.Add(new XAttribute(nameof(GistReferenceItem.Version), item.Version));
            }

            if (item.FilePattern is not null)
            {
                element.Add(new XAttribute(nameof(GistReferenceItem.FilePattern), item.FilePattern));
            }

            if (item.OutputPath is not null)
            {
                element.Add(new XAttribute(nameof(GistReferenceItem.OutputPath), item.OutputPath));
            }

            itemGroup.Add(element);
        }
    }

    public void DeleteGistReference(GistReferenceItem item)
    {
        if (IsSdkProject)
        {
            var ns = _document.Value.Root?.Name.Namespace ?? string.Empty;
            var itemGroups = _document.Value.Root?.Elements(ns + GistReferenceContainerName);
            if (itemGroups is not null)
            {
                foreach (var itemGroup in itemGroups)
                {
                    var elements = itemGroup.Elements(ns + GistReferenceName);
                    foreach (var element in elements)
                    {
                        var id = element.Attribute(nameof(GistReferenceItem.Id))?.Value;
                        var version = element.Attribute(nameof(GistReferenceItem.Version))?.Value;
                        if (id == item.Id && version == item.Version)
                        {
                            element.Remove();
                            if (!itemGroup.Elements().Any())
                            {
                                itemGroup.Remove();
                            }

                            return;
                        }
                    }
                }
            }
        }
    }

    public void Save()
    {
        System.IO.File.WriteAllText(_filepath, _document.ToString());
    }
}

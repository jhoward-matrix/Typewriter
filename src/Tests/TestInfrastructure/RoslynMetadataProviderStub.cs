using EnvDTE;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Linq;
using Typewriter.Configuration;
using Typewriter.Metadata.Interfaces;
using Typewriter.Metadata.Providers;
using Typewriter.Metadata.Roslyn;

namespace Typewriter.Tests.TestInfrastructure
{
  public class RoslynMetadataProviderStub : IMetadataProvider
  {
    private readonly Microsoft.CodeAnalysis.Workspace workspace;

    public RoslynMetadataProviderStub(DTE dte)
    {
      var solutionPath = dte.Solution.FullName;
      var msBuildWorkspace = MSBuildWorkspace.Create();

      // ReSharper disable once UnusedVariable
      var solution = msBuildWorkspace.OpenSolutionAsync(solutionPath).Result;

      this.workspace = msBuildWorkspace;
    }

    public IFileMetadata GetFile(string path, Settings settings, Action<string[]> requestRender)
    {
      var document = workspace.CurrentSolution.GetDocumentIdsWithFilePath(path).FirstOrDefault();
      if (document != null)
      {
        return new RoslynFileMetadata(workspace.CurrentSolution.GetDocument(document), settings, requestRender);
      }

      return null;
    }
  }
}

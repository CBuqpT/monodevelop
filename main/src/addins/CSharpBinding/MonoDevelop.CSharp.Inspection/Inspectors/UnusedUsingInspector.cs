// 
// UnusedUsingInspector.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2011 Novell, Inc (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using ICSharpCode.NRefactory.CSharp;
using MonoDevelop.Core;
using MonoDevelop.Refactoring;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.CSharp.Refactoring.RefactorImports;
using MonoDevelop.Inspection;

namespace MonoDevelop.CSharp.Inspection
{
	public class UnusedUsingInspector : CSharpInspector
	{
		public override string Category {
			get {
				return DefaultInspectionCategories.Redundancies;
			}
		}

		public override string Title {
			get {
				return GettextCatalog.GetString ("Remove unused usings");
			}
		}

		public override string Description {
			get {
				return GettextCatalog.GetString ("Removes used declarations that are not required.");
			}
		}

		protected override void Attach (ObservableAstVisitor<InspectionData, object> visitor)
		{
			visitor.UsingDeclarationVisited += HandleVisitorUsingDeclarationVisited;
		}
		
		void HandleVisitorUsingDeclarationVisited (UsingDeclaration node, InspectionData data)
		{
			if (!data.Graph.Navigator.GetsUsed (data.Graph.CSharpResolver, node.StartLocation, node.Namespace)) {
				AddResult (data,
					new DomRegion (node.StartLocation, node.EndLocation),
					GettextCatalog.GetString ("Remove unused usings"),
					delegate {
						var options = new RefactoringOptions (data.Document);
						new RemoveUnusedImportsRefactoring ().Run (options);
					}
				);
			}
		}
	}
}


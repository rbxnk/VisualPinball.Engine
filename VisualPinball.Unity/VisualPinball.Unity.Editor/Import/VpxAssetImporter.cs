// ReSharper disable UnusedType.Global

using System.IO;
using NLog;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using VisualPinball.Unity.Editor.Import.AssetHandler;
using VisualPinball.Unity.Import;
using VisualPinball.Unity.Import.Job;
using Logger = NLog.Logger;

namespace VisualPinball.Unity.Editor.Import
{
	[ScriptedImporter(2, "vpx")]
	public class VpxAssetImporter : ScriptedImporter
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public override void OnImportAsset(AssetImportContext ctx)
		{
			Logger.Info("Importing VPX table at {0}...", ctx.assetPath);

			// create root object
			var rootGameObj = new GameObject();
			var importer = rootGameObj.AddComponent<VpxImporter>();

			// load table
			var table = TableLoader.LoadTable(ctx.assetPath);

			// instantiate asset handler
			var assetHandler = new AssetImportHandler(ctx);

			importer.Import(Path.GetFileName(ctx.assetPath), table, assetHandler);

			ctx.AddObjectToAsset("main obj", rootGameObj);
			ctx.SetMainObject(rootGameObj);

			// select imported object
			Selection.activeObject = rootGameObj;
		}
	}
}

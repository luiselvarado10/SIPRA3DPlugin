
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;
using System.Collections.Generic;

[assembly: CommandClass(typeof(SIPRA3DPlugin.SIPRACommand))]

namespace SIPRA3DPlugin
{
    public class SIPRACommand
    {
        [CommandMethod("SIPRA_ESFERA")]
        public void Ejecutar()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            PromptDoubleOptions radioOpt = new PromptDoubleOptions("\nIngrese el radio de la esfera:");
            radioOpt.DefaultValue = 30;
            PromptDoubleResult radioRes = ed.GetDouble(radioOpt);
            if (radioRes.Status != PromptStatus.OK) return;
            double radio = radioRes.Value;

            List<Point3d> puntos = new List<Point3d>();
            PromptPointOptions ptoOpt = new PromptPointOptions("\nSeleccione una punta (ESC para terminar):");
            while (true)
            {
                PromptPointResult ptoRes = ed.GetPoint(ptoOpt);
                if (ptoRes.Status != PromptStatus.OK) break;
                puntos.Add(ptoRes.Value);
                ptoOpt.Message = "\nOtra punta (ESC para terminar):";
            }

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                if (!lt.Has("APANT3D"))
                {
                    lt.UpgradeOpen();
                    LayerTableRecord newLayer = new LayerTableRecord
                    {
                        Name = "APANT3D",
                        Color = Color.FromColorIndex(ColorMethod.ByAci, 3)
                    };
                    lt.Add(newLayer);
                    tr.AddNewlyCreatedDBObject(newLayer, true);
                }

                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                foreach (Point3d p in puntos)
                {
                    Solid3d esfera = new Solid3d();
                    esfera.CreateSphere(radio);
                    esfera.TransformBy(Matrix3d.Displacement(Point3d.Origin.GetVectorTo(p)));
                    esfera.Layer = "APANT3D";
                    btr.AppendEntity(esfera);
                    tr.AddNewlyCreatedDBObject(esfera, true);
                }

                tr.Commit();
            }

            ed.WriteMessage($"\nSe crearon {puntos.Count} esferas en la capa APANT3D.");
        }
    }
}

/*
   Copyright 2009 Canonic Corp

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
using System;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Canonic.EAPlugin.Utility
{
    /// <summary>
    /// Represents formalized model stereotypes as defined by the Canonic Model Driven Business(sm) approach.
    /// </summary>
    public enum CanonicStereotype
    {
        /// <summary>
        /// No Canonic stereotype applies.
        /// </summary>
        None = 0,
        /// <summary>
        /// Business Service
        /// </summary>
        BusinessService = 1,
        /// <summary>
        /// Logical Service
        /// </summary>
        LogicalService = 2
    }

    /// <summary>
    /// Represents the formalized, role-oriented views defined by the Canonic Model Driven Business(sm) approach.
    /// </summary>
    public enum CanonicView
    {   
        /// <summary>
        /// Used to indicate that a package does not directly represent a Canonic View
        /// </summary>
        None = 0,
        /// <summary>
        /// Business Strategy View
        /// </summary>
        BusinessStrategy = 1,
        /// <summary>
        /// Business Analysis View
        /// </summary>
        BusinessAnalysis = 2,
        /// <summary>
        /// Systems Analysis View
        /// </summary>
        SystemsAnalysis = 3,
        /// <summary>
        /// Engineering View
        /// </summary>
        Engineering = 4,
        /// <summary>
        /// Quality Assurance View
        /// </summary>
        QualityAssurance = 5
    }

    /// <summary>
    /// This class provides a collection of utilities that simplify work with EA.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Indicates no package selected.
        /// </summary>
        public const string NOPACKAGE = "(none)";

        private Util()
        {
        }

        #region Originally from Eric

        /// <summary>
        /// Gets the current package from EA based on the context that the plugin was invoked from (i.e. from
        /// the tree view or from a diagram).
        /// </summary>
        /// <param name="rep">The current repository.</param>
        /// <returns>The current package, or null if no package is currently selected.</returns>
        static public EA.Package PackageFromRepository(EA.Repository rep)
        {
            EA.Package currentPackage = null;
            EA.Element currentElement = null;

            EA.ObjectType ot = rep.GetTreeSelectedItemType();
            switch (ot)
            {
                case EA.ObjectType.otPackage:
                    currentPackage = rep.GetTreeSelectedPackage();
                    break;
                case EA.ObjectType.otElement:
                    currentElement = (EA.Element)rep.GetTreeSelectedObject();
                    currentPackage = rep.GetPackageByID(currentElement.PackageID);
                    break;
                case EA.ObjectType.otDiagram:
                    EA.Diagram currentDiagram = (EA.Diagram)rep.GetTreeSelectedObject();
                    currentPackage = rep.GetPackageByID(currentDiagram.PackageID);
                    break;
                case EA.ObjectType.otNone:
                    // Happens from main menu.  Returning NULL is OK.
                    break;
                default:
                    MessageBox.Show("There was an error while computing the package that the " +
                        "selected item is contained in.  Please report the error to info@canoniccorp.com.\n\n" +
                        "The error is: Util.PackageFromRepository() attempted to process object type " + (int)ot,
                        "Canonic Plugin Error");
                    break;
            }

            return currentPackage;
        }

        /// <summary>
        /// Determines the currently selected package and then returns its name.
        /// </summary>
        /// <param name="rep">The current EA repository.</param>
        /// <returns>Name of the current package.</returns>
        static public string PackageNameFromRepository(EA.Repository rep)
        {
            EA.Package package = PackageFromRepository(rep);
            if (package != null)
            {
                return package.Name;
            }
            else
            {
                return Util.NOPACKAGE;
            }
        }

        /// <summary>
        /// Builds the package path.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="currentPackage">The current package.</param>
        /// <returns></returns>
        static public string BuildPackagePath(EA.Repository rep, EA.Package currentPackage)
        {
            StringBuilder sb = new StringBuilder(currentPackage.Name);

            if (currentPackage.ParentID > 0)
            {
                EA.Package parentPackage = rep.GetPackageByID(currentPackage.ParentID);

                for (; ; )
                {
                    sb.Insert(0, parentPackage.Name + "/");
                    if (parentPackage.ParentID == 0) break;
                    parentPackage = rep.GetPackageByID(parentPackage.ParentID);
                }
            }

            sb.Insert(0, "/");

            return sb.ToString();
        }

        /// <summary>
        /// Reflects the specified object.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="t">The type of the object.</param>
        /// <returns>A string representing the member types and member names
        /// in the given type, in the format "TYPE NAME = value\n"
        /// with one line per member.</returns>
        static public string Reflect(object o, Type t)
        {
            StringBuilder sb = new StringBuilder();

            MemberInfo[] myMemberInfoArray = t.GetMembers();

            for (int counter = 0; counter < myMemberInfoArray.GetLength(0); counter++)
            {
                string val = "value";
                sb.AppendFormat("{0} {1} = {2}\n",
                    myMemberInfoArray[counter].MemberType.ToString(),
                    myMemberInfoArray[counter].Name,
                    val);
            }

            return sb.ToString();
        }

        /// <summary>
        /// This routine builds a string representing the entire path of a package as a slash delimeted
        /// string, such as /root/pack/subpack/mypack
        /// </summary>
        /// <param name="rep">The current repository.</param>
        /// <param name="pack">The package for which a path is required.</param>
        /// <returns>The path to the package as a slash-delimeted string.</returns>
        static public string PackagePath(EA.Repository rep, EA.Package pack)
        {
            StringBuilder sb = new StringBuilder();


            sb.Insert(0, "/" + pack.Name);

            if (pack.ParentID != 0)
            {
                EA.Package currentPack = rep.GetPackageByID(pack.ParentID);
                while (currentPack.ParentID != 0)
                {
                    sb.Insert(0, "/" + currentPack.Name);
                    currentPack = rep.GetPackageByID(currentPack.ParentID);
                }
                sb.Insert(0, "/" + currentPack.Name); // Name of highest-level package, e.g. "/Model"

            }

            return sb.ToString();

        }

        static private string ModelNameFromPath(EA.Repository rep, string path)
        {
            string[] parts = path.Split(new char[] { '/' });
            if (parts[0] == string.Empty)
            {
                return parts[1];
            }
            else
            {
                return parts[0];
            }
        }

        /// <summary>
        /// Ensures that a given package, as specified by a path string, is returned to the caller.  If
        /// the package already exists it is returned, otherwise it is created.
        /// </summary>
        /// <param name="rep">Current EA repository</param>
        /// <param name="basePackageName">Base path (leading slash, no trailing slash).  The basePackageName must contain at least the name of the model, e.g. '/Model'.</param>
        /// <param name="subpackageName">Relative path (no leading or trailing slash)</param>
        /// <returns>The package that was found or created.</returns>
        /// <remarks>Base path and relative path are provided for convenience.  Internally they are
        /// concatinated to derive the full path being returned.</remarks>
        static public EA.Package FindOrCreateSubpackage(EA.Repository rep, string basePackageName, string subpackageName)
        {

            if (basePackageName.Length < 2)
            {
                throw new ApplicationException("The basePackageName must contain at least the name of the model, e.g. '/Model'.");
            }

            EA.Package pack;// = rep.GetPackageByID(1);
            string modelName = ModelNameFromPath(rep, basePackageName);
            EA.Package model = (EA.Package)rep.Models.GetByName(modelName);
            pack = rep.GetPackageByID(model.PackageID);

            string fullPath = basePackageName + "/" + subpackageName;
            fullPath = fullPath.Replace("//", "/"); // In case we get an extra one in the middle...
            return FindOrCreateSubpackage(rep, pack, fullPath);
        }

        /// <summary>
        /// Ensures that a given package, as specified by a path string, is returned to the caller.  If
        /// the package already exists it is returned, otherwise it is created.
        /// </summary>
        /// <param name="rep">Current EA repository</param>
        /// <param name="basePackage">EA Package in which to find/create the subpackage.</param>
        /// <param name="subpackageName">Name of the subpackage (path string) in question.</param>
        /// <returns>Package found or created.</returns>
        /// <remarks>If the package passed in is a Model, the subpackageName may be an absolute path (with a leading
        /// slash) or a path relative to the Model.  In EA, a Model is a top-level package.  An EA repository can
        /// have multiple Model nodes in it, though the Canonic approach calls for only a single Model node.</remarks>
        static public EA.Package FindOrCreateSubpackage(EA.Repository rep, EA.Package basePackage, string subpackageName)
        {
            EA.Package pack = null;

            string[] packageParts = subpackageName.Split(new char[] { '/' });
            EA.Package newBasePackage = basePackage;

            // The start index will always be at least one to skip the current package (actually the "Model" in EA).
            // May be 2 if there is a leading slash in the package name, which causes the first packagePart to be 
            // string.Empty.
            int start;

            if (basePackage.ParentID == 0)
            {
                // We were passed in a model rather than a true package
                if (packageParts[0] == string.Empty)
                {
                    // We were passed an absolute path.  Skip "empty" part and model name.
                    start = 2;
                }
                else
                {
                    // We were passed a path relative to the model
                    start = 0;
                }
            }
            else
            {
                if (packageParts[0] == string.Empty)
                {
                    // We were passed an absolute path.  Not allowed if passed a non-model package.
                    throw new ApplicationException("Must provide a relative path when a non-model package is used as the root.");
                }
                else
                {
                    // We were passed a path relative to the model.  There should be no empty part since there is no leading slash.
                    start = 0;
                }
            }

            for (int i = start; i < packageParts.GetLength(0); i++)
            {
                string part = packageParts[i];
                pack = FindOrCreateSubpackageOneLevel(rep,
                    newBasePackage, part);
                newBasePackage = pack;
            }

            return pack;
        }

        static private EA.Package FindOrCreateSubpackageOneLevel(EA.Repository rep, EA.Package basePackage, string subpackageName)
        {
            foreach (EA.Package p in basePackage.Packages)
            {
                if (p.Name == subpackageName)
                {
                    return p;
                }
            }

#if DEBUG
            Debug.WriteLine("Base package: " + Util.PackagePath(rep, basePackage));
#endif

            EA.Package subpackage = (EA.Package)basePackage.Packages.AddNew(subpackageName, "otPackage");

            subpackage.Update();
            basePackage.Update();

#if DEBUG
            Debug.WriteLine("New subpackage: " + Util.PackagePath(rep, subpackage));
#endif

            return subpackage;
        }

        /// <summary>
        /// Determines whether a package is currently selected in the repository.
        /// </summary>
        /// <param name="rep">The repository.</param>
        /// <returns>
        /// 	<c>true</c> if a package is currently selected in the repository; otherwise, <c>false</c>.
        /// </returns>
        static public bool IsPackageSelected(EA.Repository rep)
        {
            return (Util.PackageFromRepository(rep) != null);
        }

        #endregion

        #region Originally from Per

        /// <summary>
        /// Locates an item in the Project Browser tree-view based on the GUID
        /// </summary>
        /// <param name="Rep">Current EA repository</param>
        /// <param name="GUID">GUID of item you want to go to in the tree-view</param>
        public static void GotoItemInTreeViewByGUID(ref EA.Repository Rep, string GUID)
        {
            object _tmp;
            EA.Attribute _attribute;
            EA.Diagram _diagram = null;
            EA.Element _element;
            EA.Method _method;
            EA.Package _package;

            try
            {
                //Here we try to find the correct object by GUID independent of the type of Object
                _attribute = Rep.GetAttributeByGuid(GUID);
                try //Only this method raises errors when GUID not found
                {
                    _tmp = Rep.GetDiagramByGuid(GUID);
                    _diagram = (EA.Diagram)_tmp;
                }
                catch (Exception)
                {
                }
                //TODO:Note for some reason a Package GUID returns an Element object (as well as a Package object). Due to the order of the if statement below it still works for now....
                _element = Rep.GetElementByGuid(GUID);
                _method = Rep.GetMethodByGuid(GUID);
                _package = Rep.GetPackageByGuid(GUID);

                //Here we cast it to Object and go to the right item/object in th tree view
                Object _object = new Object();

                if (_attribute != null)
                    _object = _attribute;
                if (_diagram != null)
                    _object = _diagram;
                if (_element != null)
                    _object = _element;
                if (_method != null)
                    _object = _method;
                if (_package != null)
                    _object = _package;

                Rep.ShowInProjectView(_object);

                //Verify that we found something in tree-view
                EA.ObjectType _itemType = EA.ObjectType.otNone;
                string _itemName = string.Empty;
                string _type = string.Empty;
                string _stereoType = string.Empty;
                int _packageId = 0;
                string _GUID = string.Empty;
                int _objectId = 0;
                string _stereoTypeEx = string.Empty;
                string _status = string.Empty;

                Util.GetItemDetails(ref Rep, out _itemType, out _itemName, out _type,
                            out _stereoType, out _packageId, out _GUID, out _objectId, out _stereoTypeEx, out _status);

                if (GUID != _GUID)
                {
                    ApplicationException ae = new ApplicationException("Unable to locate item by GUID [" + GUID + "]");
                    throw (ae);
                }
            }
            catch (Exception)
            {
                ApplicationException ae = new ApplicationException("Unable to locate item by GUID [" + GUID + "]");
                throw (ae);


            }
        }

        /// <summary>
        /// Gets the item details.
        /// </summary>
        /// <param name="Rep">The rep.</param>
        /// <param name="ItemType">Type of the item.</param>
        /// <param name="ItemName">Name of the item.</param>
        /// <param name="Type">The type.</param>
        /// <param name="StereoType">Type of the stereo.</param>
        /// <param name="GUID">The GUID.</param>
        public static void GetItemDetails(ref EA.Repository Rep, out EA.ObjectType ItemType, out string ItemName, out string Type, out string StereoType,
            out string GUID)
        {
            ItemName = string.Empty;
            Type = string.Empty;
            StereoType = string.Empty;
            int _packageId = 0;
            GUID = string.Empty;
            int _objectId = 0;
            string _stereoTypeEx = string.Empty;
            string _status = string.Empty;

            GetItemDetails(ref Rep, out ItemType, out ItemName, out Type, out StereoType, out _packageId, out GUID, out _objectId, out _stereoTypeEx, out _status);

        }

        /// <summary>
        /// Get details for the item selected in the Project Browser tree-view or in a Diagram. See the "GetContextItem" method in the EA Repository API.
        /// </summary>
        /// <param name="Rep"></param>
        /// <param name="ItemType">The type of the object type (i.e. EA.ObjectType) </param>
        /// <param name="ItemName"></param>
        /// <param name="Type"></param>
        /// <param name="StereoType"></param>
        /// <param name="PackageId"></param>
        /// <param name="GUID"></param>
        /// <param name="ObjectId"></param>
        /// <param name="StereoTypeEx"></param>
        /// <param name="Status">Applicable to Elements and specific to Canonic also for Methods where the Style/Alias is used</param>
        public static void GetItemDetails(ref EA.Repository Rep, out EA.ObjectType ItemType, out string ItemName, out string Type, out string StereoType,
    out int PackageId, out string GUID, out int ObjectId, out string StereoTypeEx, out string Status)
        {
            ItemName = string.Empty;
            Type = string.Empty;
            StereoType = string.Empty;
            PackageId = 0;
            GUID = string.Empty;
            ObjectId = 0;
            StereoTypeEx = string.Empty;
            Status = string.Empty;

            ItemType = Rep.GetContextItemType();

            object _item;
            Rep.GetContextItem(out _item);
            //Rep.GetTreeSelectedItem(out _item);
            EA.Attribute _attribute;
            EA.Connector _connector;
            EA.Element _element;
            EA.Diagram _diagram;
            EA.Method _method;
            EA.Package _package;

            //TODO: Need to add support for Attributes

            switch (ItemType)
            {
                case EA.ObjectType.otAttribute:
                    _attribute = (EA.Attribute)_item;
                    ItemName = _attribute.Name;
                    Type = _attribute.Type;
                    StereoType = _attribute.Stereotype;
                    PackageId = 0;
                    GUID = _attribute.AttributeGUID;
                    ObjectId = _attribute.AttributeID;
                    StereoTypeEx = _attribute.StereotypeEx;
                    Status = string.Empty; //N/A to attributes
                    break;
                case EA.ObjectType.otConnector:
                    _connector = (EA.Connector)_item;
                    ItemName = _connector.Name;
                    Type = _connector.Type;
                    StereoType = _connector.Stereotype;
                    PackageId = 0;
                    GUID = _connector.ConnectorGUID;
                    ObjectId = _connector.ConnectorID;
                    StereoTypeEx = _connector.StereotypeEx;
                    Status = string.Empty; //N/A to attributes
                    break;
                case EA.ObjectType.otElement:
                    _element = (EA.Element)_item;
                    ItemName = _element.Name;
                    Type = _element.Type;
                    StereoType = _element.Stereotype;
                    PackageId = _element.PackageID;
                    GUID = _element.ElementGUID;
                    ObjectId = _element.ElementID;
                    StereoTypeEx = _element.StereotypeEx;
                    Status = _element.Status;
                    //TODO: Add details to find out if this is an Operation of an Element (a child node)

                    break;
                case EA.ObjectType.otDiagram:
                    _diagram = (EA.Diagram)_item;
                    ItemName = _diagram.Name;
                    Type = _diagram.Type;
                    StereoType = _diagram.Stereotype;
                    PackageId = _diagram.PackageID;
                    GUID = _diagram.DiagramGUID;
                    ObjectId = _diagram.DiagramID;
                    StereoTypeEx = _diagram.StereotypeEx;
                    Status = string.Empty; //N/A to Diagrams
                    break;
                case EA.ObjectType.otMethod:
                    _method = (EA.Method)_item;
                    ItemName = _method.Name;
                    Type = string.Empty;
                    StereoType = _method.Stereotype;
                    PackageId = 0;
                    GUID = _method.MethodGUID;
                    ObjectId = _method.MethodID;
                    StereoTypeEx = _method.StereotypeEx;
                    Status = _method.Style; //Specific to Canonic, also referred to as Alias in UI
                    break;
                case EA.ObjectType.otPackage:
                    _package = (EA.Package)_item;
                    ItemName = _package.Name;
                    Type = string.Empty;
                    StereoType = string.Empty;
                    PackageId = _package.PackageID;
                    GUID = _package.PackageGUID;
                    ObjectId = _package.PackageID;
                    StereoTypeEx = _package.StereotypeEx;
                    Status = string.Empty; //N/A
                    break;
                default:
                    //TODO: Log that we did not find objecttype
                    break;
            }
            _package = null;
            _diagram = null;


        }
        #endregion

        #region New Methods
        /// <summary>
        /// Determines whether the given package has an ancestor package of a given name.
        /// </summary>
        /// <param name="rep">EA repository.</param>
        /// <param name="package">Package whose ancestor is being checked.</param>
        /// <param name="ancestorName">Name of the ancestor.</param>
        /// <returns>True if package has an ancestor matching the given name.</returns>
        /// <remarks>Often used to determine whether a package is in a certain part of the model.</remarks>
        static public bool HasAncestorPackage(EA.Repository rep, EA.Package package, string ancestorName)
        {
            EA.Package currentPackage = rep.GetPackageByID(package.ParentID);

            while (!currentPackage.IsModel)
            {
                if (currentPackage.Name == ancestorName)
                {
                    return true;
                }
                currentPackage = rep.GetPackageByID(package.ParentID);
            }

            return false;
        }

        /// <summary>
        /// Adds a tagged value to the given element if the tagged value does not exist on that element,
        /// otherwise updates the existing value.
        /// </summary>
        /// <param name="rep">EA repository.</param>
        /// <param name="elem">Element whose tagged value list is to be updated.</param>
        /// <param name="tag">Name of the tagged value.</param>
        /// <param name="value">Value to set.</param>
        static public void AddOrUpdateTaggedValue(EA.Repository rep, EA.Element elem, string tag, string value)
        {

            elem.TaggedValues.Refresh();

            for (short i = 0; i < elem.TaggedValues.Count; i++)
            {
                object o = elem.TaggedValues.GetAt(i);
                EA.TaggedValue tv = (EA.TaggedValue)o;

                if (tv.Name == tag)
                {
                    elem.TaggedValues.Delete(i);
                    break;
                }
            }

            elem.Update();

            EA.TaggedValue tvNew = (EA.TaggedValue)elem.TaggedValues.AddNew(tag, "");
            tvNew.Value = value;
            tvNew.Update();

            elem.Update();
        }

        /// <summary>
        /// Get a subset of the EA connection string so it can be used as a ConnectionString in C#. See also 'GetOleDbConnectionString'. 
        /// Example of ConnectionString, passed in: 
        /// 'EaSqlServerProject --- DBType=1;Connect=Provider=SQLOLEDB.1;Password=...' 
        /// Returns: 
        /// 'Password=...' 
        /// </summary>
        /// <param name="EAConnectionString">ConnectionString from EA Repository. Typically retrieved from the EA API using 'ConnectionString' property of an instance of EA.Repository.</param>
        /// <returns></returns>
        [Obsolete("Moved to Canonic.Core.Database.Support")]
        static public string GetConnectionString(string EAConnectionString)
        {
            //

            string newCon = string.Empty;

            try
            {
                string[] keypairs = EAConnectionString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string keypair in keypairs)
                {
                    if (!keypair.Contains("---"))
                    {
                        if (!keypair.StartsWith("Connect=Provider="))
                        {
                            newCon += keypair + ";";
                        }
                    }
                }
                //If the incoming string does NOT end with a ; then remove it from the outstring
                if (EAConnectionString.Substring(EAConnectionString.Length - 1, 1) != ";")
                {
                    newCon = newCon.Remove(newCon.Length - 1);
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show("Failed to dB Connection string from EA connection string." + exc.Message);
                return null;
            }
            return newCon;
        }

        /// <summary>
        /// Deletes object from from the EA collection by ID.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="coll">The coll.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        static public bool DeleteFromEaCollection(EA.Repository rep, EA.Collection coll, int id)
        {
            for (short i = 0; i < coll.Count; i++)
            {
                if (coll.ObjectType == EA.ObjectType.otElement)
                {
                    EA.Element elem = (EA.Element)coll.GetAt(i);
                    if (elem.ElementID == id)
                    {
                        coll.DeleteAt(i, true);
                        return true;
                    }
                }
                else if (coll.ObjectType == EA.ObjectType.otDiagramObject)
                {
                    EA.DiagramObject dgo = (EA.DiagramObject)coll.GetAt(i);
                    if (dgo.InstanceID == id)
                    {
                        coll.DeleteAt(i, true);
                        return true;
                    }
                }
                else
                {
                    throw new Exception("Unsupported object type " + coll.ObjectType);
                }

            }

            return false;
        }

        /// <summary>
        /// Finds the canonic view.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        static public EA.Package FindCanonicView(EA.Repository rep, CanonicView view)
        {
            if (rep.Models.Count != 1)
            {
                throw new ApplicationException("Expected exactly one model in the repository.");
            }

            EA.Package model = (EA.Package)rep.Models.GetAt(0);

            foreach (EA.Package pack in model.Packages)
            {
                switch (view)
                {
                    #region Views
                    case CanonicView.BusinessStrategy:
                        if (pack.Name == "Business Strategy View")
                        {
                            return pack;
                        }
                        break;
                    case CanonicView.BusinessAnalysis:
                        if (pack.Name == "Business Analysis View")
                        {
                            return pack;
                        }
                        break;
                    case CanonicView.SystemsAnalysis:
                        if (pack.Name == "Systems Analysis View")
                        {
                            return pack;
                        }
                        break;
                    case CanonicView.Engineering:
                        if (pack.Name == "Engineering View")
                        {
                            return pack;
                        }
                        break;
                    case CanonicView.QualityAssurance:
                        if (pack.Name == "Quality Assurance View")
                        {
                            return pack;
                        }
                        break;
                    #endregion
                }
            }

            throw new ApplicationException("Expected Canonic package not found.");
        }

        static private CanonicView FindCanonicViewOfPackageIfAny(EA.Repository rep, EA.Package pack)
        {
            switch (pack.Name)
            {
                #region Views
                case "Business Strategy View":
                    return CanonicView.BusinessStrategy;

                case "Business Analysis View":
                    return CanonicView.BusinessAnalysis;

                case "Systems Analysis View":
                    return CanonicView.SystemsAnalysis;

                case "Engineering View":
                    return CanonicView.Engineering;

                case "Quality Assurance View":
                    return CanonicView.QualityAssurance;
                default:
                    return CanonicView.None;
                #endregion
            }
        }

        private static CanonicView FindCanonicViewOfEAEntityFromPackage(EA.Repository rep, EA.Package pack)
        {
            do
            {
                CanonicView cv = FindCanonicViewOfPackageIfAny(rep, pack);
                if (cv != CanonicView.None)
                {
                    return cv;
                }

                pack = rep.GetPackageByID(pack.ParentID);
            }
            while (pack.ParentID != 0);

            return CanonicView.None;
        }

        /// <summary>
        /// Finds the canonic view of element.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="elem">The elem.</param>
        /// <returns></returns>
        static public CanonicView FindCanonicViewOfElement(EA.Repository rep, EA.Element elem)
        {
            EA.Package pack = rep.GetPackageByID(elem.PackageID);

            return FindCanonicViewOfEAEntityFromPackage(rep, pack);
        }

        /// <summary>
        /// Finds the canonic view of diagram.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="diag">The diag.</param>
        /// <returns></returns>
        static public CanonicView FindCanonicViewOfDiagram(EA.Repository rep, EA.Diagram diag)
        {
            EA.Package pack = rep.GetPackageByID(diag.PackageID);

            return FindCanonicViewOfEAEntityFromPackage(rep, pack);
        }

        /// <summary>
        /// Finds the diagram object by element GUID.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="diagram">The diagram.</param>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        static public EA.DiagramObject FindDiagramObjectByElementGuid(EA.Repository rep, EA.Diagram diagram, string guid)
        {
            EA.Element elem = rep.GetElementByGuid(guid);

            foreach (EA.DiagramObject dgo in diagram.DiagramObjects)
            {
                if (dgo.ElementID == elem.ElementID)
                {
                    return dgo;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the diagram object by id.
        /// </summary>
        /// <param name="rep">The rep.</param>
        /// <param name="diagram">The diagram.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        static public EA.DiagramObject FindDiagramObjectById(EA.Repository rep, EA.Diagram diagram, int id)
        {
            foreach (EA.DiagramObject dgo in diagram.DiagramObjects)
            {
#if DEBUG
                EA.Element tmpElem = rep.GetElementByID(dgo.ElementID);
                System.Diagnostics.Debug.WriteLine("DiagObj: " + tmpElem.Name + " on " + diagram.Name);
#endif
                if (dgo.InstanceID == id)
                {
                    return dgo;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the selected element from the given repository.
        /// </summary>
        /// <param name="rep">The repository.</param>
        /// <returns></returns>
        static public EA.Element SelectedElement(EA.Repository rep)
        {
            object ret = null;

            EA.ObjectType ot = rep.GetContextItem(out ret);

            if (ot == EA.ObjectType.otElement)
            {
                return (EA.Element)ret;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines whether the package or any of its ancestors has the given stereotype.
        /// </summary>
        /// <param name="rep">EA Repository</param>
        /// <param name="pack">EA Package</param>
        /// <param name="stereotype">Canonic stereotype of package or ancestor package.</param>
        /// <returns>True if the package or any of its ancestors has the given canonic stereotype.</returns>
        static public bool PackageOrParentHasStereotype(EA.Repository rep, EA.Package pack,
            CanonicStereotype stereotype)
        {
            string debugInfo = string.Empty;

            return PackageOrParentHasStereotype(rep, pack, stereotype, ref debugInfo);
        }
        /// <summary>
        /// Determines whether the package or any of its ancestors has the given stereotype.
        /// </summary>
        /// <param name="rep">EA Repository</param>
        /// <param name="pack">EA Package</param>
        /// <param name="stereotype">Canonic stereotype of package or ancestor package.</param>
        /// <param name="debugInfo">Added to return debugging info to a RELEASE environment</param>
        /// <returns>True if the package or any of its ancestors has the given canonic stereotype.</returns>
        static public bool PackageOrParentHasStereotype(EA.Repository rep, EA.Package pack,
            CanonicStereotype stereotype, ref string debugInfo)
        {
            string stereo;
            switch (stereotype)
            {
                case CanonicStereotype.None:
                    stereo = string.Empty;
                    break;
                case CanonicStereotype.BusinessService:
                    stereo = "Business Service";
                    break;
                case CanonicStereotype.LogicalService:
                    stereo = "Logical Service";
                    break;
                default:
                    stereo = string.Empty;
                    break;
            }

            debugInfo += "PackageOrParentHasStereotype() name '" + pack.Name + "' seek '" +
                stereo + "', pack has '" + pack.StereotypeEx + "'";

            if (pack.Element.Stereotype == stereo)
            {
                return true;
            }
            else
            {
                EA.Package parent = rep.GetPackageByID(pack.ParentID);
                while (parent != null && parent.ParentID != 0)
                {
                    debugInfo += Environment.NewLine + "    PARENT name '" + parent.Name + "' seek '" +
                        stereo + "', pack has '" + parent.Element.Stereotype + "'";
                    if (parent.Element.Stereotype == stereo)
                    {
                        return true;
                    }
                    parent = rep.GetPackageByID(parent.ParentID);
                }
                return false;
            }
        }
        #endregion

        #region Added by Per, 6 April 2008
        /// <summary>
        /// Get a subset of the EA connection string so it can be used as a OleDb ConnectionString in C#. See also 'GetConnectionString'. 
        /// Example of ConnectionString, passed in: 
        /// 'EaSqlServerProject --- DBType=1;Connect=Provider=SQLOLEDB.1;Password=...' 
        /// Returns: 
        /// 'Provider=SQLOLEDB.1;Password=...' 
        /// ApplicationException is thrown if we fail the process of trying to generate the connectionstring.
        /// There is no validation that the return connection string is correct, one has to try to connect to the dB to verify that.
        /// </summary>
        /// <param name="EAConnectionString">ConnectionString from EA Repository. Typically retrieved from the EA API using 'ConnectionString' property of an instance of EA.Repository.</param>
        /// <returns>sdad</returns>
        [Obsolete("Moved to Canonic.Core.Database.Support")]
        static public string GetOleDbConnectionString(string EAConnectionString)
        {
            string newCon = string.Empty;

            try
            {
                string[] keypairs = EAConnectionString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string keypair in keypairs)
                {
                    if (!keypair.Contains("---"))
                    {
                        if (!keypair.StartsWith("Connect=Provider="))
                        {
                            newCon += keypair + ";";
                        }
                        else
                        {
                            newCon = keypair.Replace("Connect=Provider=", "Provider=") + ";";
                        }
                    }
                }

                //If the incoming string does NOT end with a ; then remove it from the outstring
                if (EAConnectionString.Substring(EAConnectionString.Length - 1, 1) != ";")
                {
                    newCon = newCon.Remove(newCon.Length - 1);
                }
            }
            catch (Exception exc)
            {
                throw new ApplicationException("Failed to extract dB Connection string from EA connection string.", exc);

            }
            return newCon;
        }

        #endregion

        /// <summary>
        /// Added 3 Sep 2008/PG: 
        /// Recursive method that returns an EA.Package instance of the last package in the 'path'. 
        /// </summary>
        /// <param name="rep">EA Repository</param>
        /// <param name="package">Typically null when passing in a complete path (with a Model root)</param>
        /// <param name="path">Typically '\ModelRootOne\A View\BA Package\Requirements'</param>
        /// <returns></returns>
        static public EA.Package GetPackage(EA.Repository rep, EA.Package package, string path)
        {
            string _model = string.Empty;
            string _packageName = string.Empty;
            string _nameOfCurrentPackage = string.Empty;
            try
            {
                string[] _folders = path.Split(@"\".ToCharArray());
                if (_folders.Length != 1)
                {
                    string _path = path.Remove(0, _folders[1].Length + 1); //Path left
                    if (package == null)
                    {
                        _model = _folders[1]; //first is empty
                        package = (EA.Package)rep.Models.GetByName(_model);
                        _nameOfCurrentPackage = package.Name;
                        return GetPackage(rep, package, _path);
                    }
                    else
                    {
                        _packageName = _folders[1];
                        _nameOfCurrentPackage = package.Name;
                        package = (EA.Package)package.Packages.GetByName(_packageName);
                        if (package == null) throw new Exception("Package is null!");
                        else return GetPackage(rep, package, _path);
                    }
                }
            }
            catch (ApplicationException apexc) //This catch statement will only be 'executed' by 'parent' calls... to make sure only the original message is visible to the caller
            {
                throw new ApplicationException(apexc.Message, apexc);
            }
            catch (Exception exc)
            {
                throw new ApplicationException("Unable to find EA Model/Package named [" + _model + _packageName +
                    "]. Last package found in hierarchy was [" + _nameOfCurrentPackage + "]. " +
                    exc.Message, exc);
            }
            return package;
        }

        /// <summary>
        /// Added 4 Sep 2008/PG:
        /// Get count of items of a given type and stereotype for a given package. NOTE: It does not look and items in Child Packages.
        /// To also count items in Child packages consider 2 lines of code, like:
        /// int _countOfRequirements = GetCountOfItemsInPackage(_package, "Requirement");
        /// _countOfRequirements += GetCountOfItemsInPackages(_package.Packages, "Requirement");
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="EaObjectType">typically "Requirement", "UseCase". See EA.Element.Type.
        /// NOTE: To count test cases use "UseCase" and set EaStereoType to "TestCase"</param>
        /// <param name="EaStereotype">The EA stereotype.</param>
        /// <returns></returns>
        static public int GetCountOfItemsInPackage(EA.Package package, string EaObjectType, string EaStereotype)
        {
            switch (EaObjectType)
            {
                case "Requirement":
                    break;
                case "UseCase":
                    break;
                default:
                    throw new ApplicationException("TypeOfItem [" + EaObjectType.ToString() + "] not supported!");
            }

            int _count = 0;

            foreach (EA.Element _el in package.Elements)
            {
                if (_el.Type == EaObjectType && (EaStereotype == string.Empty || _el.Stereotype.ToLower() == EaStereotype.ToLower()))
                {
                    _count++;
                }
            }


            return _count;
        }

        /// <summary>
        /// Added 4 Sep 2008/PG:
        /// Get count of items of given type in collection of packages and their child packages
        /// </summary>
        /// <param name="packages">The packages.</param>
        /// <param name="EaObjectType">typically "Requirement", "UseCase". See EA.Element.Type</param>
        /// <param name="EaStereoType">EA Stereotype.</param>
        /// <returns></returns>
        static public int GetCountOfItemsInPackages(EA.IDualCollection packages, string EaObjectType, string EaStereoType)
        {
            int _count = 0;

            foreach (EA.Package _pack in packages)
            {
                _count += GetCountOfItemsInPackage(_pack, EaObjectType, EaStereoType);
                if (_pack.Packages.Count > 0)
                {
                    _count += GetCountOfItemsInPackages(_pack.Packages, EaObjectType, EaStereoType);
                }

            }

            return _count;
        }

    }
}

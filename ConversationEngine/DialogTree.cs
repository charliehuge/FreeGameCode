using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConversationEngine
{
    //-----------------------------------------
    // DialogNode
    //  Basic unit of a DialogTree
    //-----------------------------------------
    public class DialogNode
    {
        public string name;
        public int curText;
        public List<string> text;
        public List<Response> responses;
        public DialogAction action;

        public DialogNode()
        {
            curText = 0;
        }

        public void NextText() 
        {
            if(curText < text.Count - 1)
                curText++;
        }

        public string GetCurrentText()
        {
            return text[curText];
        }
    }

    //-----------------------------------------
    // DialogAction
    //  Triggered action in a DialogNode
    //-----------------------------------------
    public class DialogAction
    {
        public string name;
        public List<string> strParams;

        public DialogAction(string name, List<string> strParams)
        {
            this.name = name;
            this.strParams = strParams;
        }
    }

    //-----------------------------------------
    // Response
    //  A single user choice in a DialogNode
    //-----------------------------------------
    public class Response
    {
        public string text;
        public string nextNode;

        public Response(string text, string nextNode)
        {
            this.text = text;
            this.nextNode = nextNode;
        }
    }

    //-----------------------------------------
    // DialogTree
    //  Loads an XML File, stores DialogNodes
    //-----------------------------------------
    public class DialogTree
    {
        private string name;
        private List<DialogNode> nodes;

        public DialogTree(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode treeNode = doc.GetElementsByTagName("DialogTree")[0];

            name = treeNode.Attributes["name"].Value;
            Console.WriteLine("Loaded DialogTree '" + name + "'");

            // create each node in file
            nodes = new List<DialogNode>();
            foreach (XmlNode node in treeNode.ChildNodes)
            {
                DialogNode newDialogNode = new DialogNode();
                newDialogNode.name = node.Attributes["name"].Value;
                // get the actions
                if (node.Attributes["action"] != null)
                {
                    string actionName = node.Attributes["action"].Value;
                    List<string> actionParams = new List<string>();
                    // TODO: more params
                    if (node.Attributes["actionParam1"] != null)
                    {
                        actionParams.Add(node.Attributes["actionParam1"].Value);
                    }
                    newDialogNode.action = new DialogAction(actionName, actionParams);
                }
                // get the text and responses
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "Text")
                    {
                        if (newDialogNode.text == null)
                            newDialogNode.text = new List<string>();
                        newDialogNode.text.Add(child.InnerText);
                    }
                    else if (child.Name == "Response")
                    {
                        if (newDialogNode.responses == null)
                            newDialogNode.responses = new List<Response>();
                        string text = child.InnerText;
                        string nextNode = child.Attributes["nextNode"].Value;
                        newDialogNode.responses.Add(new Response(text, nextNode));
                    }
                }
                nodes.Add(newDialogNode);
            }
        }

        public DialogNode GetFirstNode()
        {
            return nodes[0];
        }

        public DialogNode GetNodeByName(string nodeName) 
        {
            foreach (DialogNode node in nodes)
            {
                if (node.name == nodeName)
                    return node;
            }

            Console.WriteLine("Node not found!");
            return null;
        }
    }
}

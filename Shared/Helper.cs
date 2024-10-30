using YamlDotNet.RepresentationModel;

namespace Shared
{
    public class Helper
    {
        public static int GetKafkaBrokerPort(string filePath)
        {
            using var input = new StreamReader(Path.Combine(filePath, @"docker-compose.yml"));
            YamlStream yaml = new YamlStream();
            yaml.Load(input);
            //
            YamlMappingNode root = (YamlMappingNode)yaml.Documents[0].RootNode;
            YamlMappingNode services = (YamlMappingNode)root.Children[new YamlScalarNode("services")];
            YamlMappingNode broker = (YamlMappingNode)services.Children[new YamlScalarNode("broker")];
            //
            YamlSequenceNode ports = (YamlSequenceNode)broker.Children[new YamlScalarNode("ports")];
            YamlScalarNode portMapping = (YamlScalarNode)ports.Children.First();
            //
            return int.Parse(portMapping.Value!.Split(":")[0]);
        }
    }
}

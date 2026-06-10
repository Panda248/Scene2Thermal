namespace JsonClasses 
{
    public class SceneCategory
    {
        public string category;
    }

    public class ObjectInference
    {
        public string category, category_reason;
        public string material;
        public string usage;
        public string size;
        public string size_reason;
    }

    public class MaterialInference
    {
        public float specific_heat;
        public float thermal_conductivity;
        public float mass;
        public float temperature;
    }

    public class ObjectMaterialInference
    {
        public string object_category, object_category_reason;
        public string material_category, material_justification;
        public string size;
        public string size_reason;
        public float heat_capacity;
        public float thermal_conductivity;
        public float mass;
        public float initial_temperature;
    }
}
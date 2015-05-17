using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractor.Common
{
    public class TvItemDescription : ItemDescription
    {
        public string Screen_Type {get; set;}
        public string Display_Size { get; set; }
        public string HD_Technology__amphasant__Resolution { get; set;}    //__amphasant to &
        public string Smart_TV { get; set; }
        public string _3D { get; set; }
        public string Series { get; set; }
        public string HDMI { get; set; }
        public string USB { get; set; }
        public string Launch_Year { get; set; }
                
        
    }


    public class LapTopItemDescription : ItemDescription
    {
        public string Color { get; set; }
        public string Part_Number { get; set; }
        
        public string Processor {get ;set;}
        public string Chipset { get; set; }
        public string Clock_Speed { get; set; }
        public string Cache { get; set; }
        public string Expandable_Memory { get; set; }
        public string System_Memory { get; set; }
        public string Memory_Slots { get; set; }
        public string HDD_Capacity { get; set; }
        public string Operating_System { get; set; }
        public string System_Architecture { get; set; }
        public string Screen_Size { get; set; }
        public string Resolution { get; set; }
        public string Screen_Type {get ;set;}
        public string Weight { get; set; }
        public string Dimension { get; set; }
    }

    public class CameraItemDescription : ItemDescription
    {
        public string Series {get ; set;}
        public string Type { get; set; }
        public string Color { get; set; }
        
        public string Processor { get; set; }
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public string Optical_Sensor_Resolution__bracketsOpen_in_MegaPixel_bracketsClosed_ { get; set; }
        public string Other_Resolution { get; set; }
        public string Optical_Zoom { get; set; }
        public string Digital_Zoom { get; set; }
        public string Focus { get; set; }
        public string LCD_Display { get; set; }
        public string LCD_Screen_Size { get; set; }

        public string Battery_Type { get; set; }

    }

    public class ACItemDescription : ItemDescription
    {
        public string Type { get; set; }
        public string Capacity_in_Tons { get; set; }
        public string Star_Rating { get; set; }
        public string Color { get; set; }
        public string Series { get; set; }
        public string Cooling_Capacity { get; set; }
        public string Compressor { get; set; }
        public string Remote_Control { get; set; }
        public string Refrigerant { get; set; }
        public string Operating_Modes { get; set; }
        public string Indoor_W_x_H_x_D { get; set; }
        public string Indoor_Unit_Weight { get; set; }
        public string Outdoor_W_x_H_x_D { get; set; }
        public string Outdoor_Unit_Weight { get; set; }
        public string Panel_Display { get; set; }

        

    }

    public class WashinigMachineItemDescription : ItemDescription
    {
        public string Type { get; set; }
        public string Function_Type { get; set; }
        public string Load_Type { get; set; }
        public string Washing_Capacity { get; set; }
        public string Washing_Method { get; set; }
        public string In_hyphen_built_Heater { get; set; }
        public string Water_Level_Selector { get; set; }
        public string Shade { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Depth { get; set; }
        public string Weight { get; set; }
        

    }

    public class FridgeItemDescription : ItemDescription
    {
        public string Type { get; set; }
        public string Refrigerator_Type { get; set; }
        public string Capacity { get; set; }
        public string Defrosting_Type { get; set; }
        public string Number_of_Doors { get; set;}
        public string Shade { get; set; }
        public string Star_Rating { get; set; }
        public string Stabilizer_Required { get; set; }
        public string Net_Width { get; set; }
        public string Net_Height { get; set; }
        public string Net_Depth { get; set; }
        public string Weight { get; set; }

        
    }


    public class MicrowaveItemDescription : ItemDescription
    {
        public string Type { get; set; }
        public string Capacity { get; set; }
        public string Shade { get; set; }
        public string Control_Type { get; set; }
        public string Frequency { get; set; }
        public string Power_Levels { get; set; }
        public string Cooking_Modes { get; set; }
        public string Power_Output { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Depth { get; set; }
        public string Turntable_Diameter {get; set;}

        

    }

    public class ItemFactory
    {
        public static ItemDescription GetItem(SubCategory objSubCategory)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            ItemDescription item = null;
         

            switch (objSubCategory.SubCategoryID)
            {
                   
                case SubCategoriesID.SUBCATEGORY_TV: 
                    item = new TvItemDescription();
                    break;
                case SubCategoriesID.SUBCATEGORY_LAPTOP:
                    item = new LapTopItemDescription();
                    break;
                case SubCategoriesID.SUBCATEGORY_CAMERA:
                    item = new CameraItemDescription();
                    break;
                case SubCategoriesID.SUBCATEGORY_AC:
                    item = new ACItemDescription();            
                    break;
                case SubCategoriesID.SUBCATEGORY_FRIDGE:
                    item = new FridgeItemDescription();
                    break;                
                case SubCategoriesID.SUBCATEGORY_WASHING_MACHINE:
                    item = new WashinigMachineItemDescription();
                    break;
                case SubCategoriesID.SUBCATEOGRY_MICROWAVES:
                    item = new MicrowaveItemDescription();
                    break;
                default: break;
            }

            return item;
        }
    }

    
}

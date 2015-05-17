package com.utility;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.app.AlertDialog.Builder;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.util.Log;

public class AppUtility {
	Context contaxt;
	static ProgressDialog pDialog=null ;
	SharedPreferences pref;
	String Preference_Name = "login";
	public static String serverurl="http://ec2-54-69-181-234.us-west-2.compute.amazonaws.com:8080/DealSheelAppServer/api/";
	public static String serverimageurl="http://ec2-54-69-181-234.us-west-2.compute.amazonaws.com:8080/";
	public static String maincaturl = serverurl+"Category";
	public static String subcatAllurl = serverurl+"SubCategory";
	public static String subcaturl = serverurl+"SubCategory?Categoryid=";
	public static String stateurl = serverurl+"Location?countryName=India";
	public static String topstoredurl = serverurl+"Store/getTopViewedStores";
	public static String topitemsurl = serverurl+"Item/getTopViewedItems";
	public static String topcaturl = serverurl+"SubCategory/getTopViewedSubCategories";
	public static String topstoreimagesurl = serverimageurl+"DealSheelImages/StoreDescription/";
	public static String itemimagesurl = serverimageurl+"DealSheelImages/ItemDescription/";
	public static String categoryimagesurl = serverimageurl+"DealSheelImages/SubCategory/";
	public static String itemstorelisturl = serverurl+"Store?ItemDescriptionId=";
	public static String itemstorelistsearchurl = serverurl+"Search/getSearchStores?ItemDescriptionId=";
	public static String itemfromsubcatlisturl = serverurl+"Item?SubCategoryId=";
	public static String itemsearchurl = serverurl+"Search?Stateid=";
	public static String filtersubcaturl = serverurl+"Filter?SubCategoryId=";
	public static String getfiltervaluesurl = serverurl+"Filter/getFilterValues?SubCategoryId=";
	public static String applyfiltersubcaturl = serverurl+"Filter/applyFilters?SubCategoryId=";
	public static String storedeatilurl = serverurl+"Store/getTopViewedStoreList?StoreDescriptionId=";
	public static String getfiltervaluesurlnoprefix = serverurl+"Filter/getFilterValues";
	public static String applyfiltersubcaturlnoattribs = serverurl+"Filter/applyFilters";
	
	public AppUtility(Context contaxt) {
		this.contaxt = contaxt;
	}
	

	public boolean setstate(String name,String id) {
		pref = contaxt.getSharedPreferences(Preference_Name, 0);
		Editor editor = pref.edit();
		editor.putString("statename", name);
		editor.putString("stateid", id);
		
		if (editor.commit()) {
			return true;
		} else {
			return false;
		}

	}
	
	
	@SuppressWarnings("deprecation")
	public static AlertDialog showAlertDialog(String title, String message,Context context) {
		try {
			AlertDialog alertDialog = new AlertDialog.Builder(context).create();
			alertDialog.setTitle(title);
			alertDialog.setMessage(message);
			alertDialog.setButton("OK", new DialogInterface.OnClickListener() {
			   public void onClick(DialogInterface dialog, int which) {
			      // TODO Add your code for the button here
				   Log.i("Alert Dilog", "clicked");
			   }
			});
			// Set the Icon for the Dialog
		//	alertDialog.setIcon(R.drawable.validationerror);
			alertDialog.show();
			return alertDialog;
			
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
		return null;
	}
	
	public String getstatename() {
		pref = contaxt.getSharedPreferences(Preference_Name, 0);
		return pref.getString("statename", "");

	}
	
	public String getstateid() {
		pref = contaxt.getSharedPreferences(Preference_Name, 0);
		return pref.getString("stateid", "");

	}
	

	public Builder showalertDialog(Context context, String title,String message) {
		AlertDialog.Builder alertDialog = new AlertDialog.Builder(context);
		alertDialog.setTitle(title);
		alertDialog.setMessage(message);
		return alertDialog;
		
	}

	public void showProgressDiloag(Context context,String message) {
		pDialog = new ProgressDialog(context);
	     pDialog.setMessage(message);
	     pDialog.setIndeterminate(false);
	     pDialog.setCancelable(false);
	     pDialog.show();
	 	
	}
	
	 public void closeProgressDialog()
	 {
		 pDialog.dismiss();
	 }

}

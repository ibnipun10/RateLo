package com.lazylist;

import java.util.ArrayList;
import java.util.HashMap;

import com.dealsheel.R;
import com.utility.AppUtility;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

public class Topstoreadapter extends BaseAdapter {
    
    private Activity activity;
    ArrayList<HashMap<String, String>> data;
    private static LayoutInflater inflater=null;
    public ImageLoader imageLoader; 
    AppUtility apputil;
    
    public Topstoreadapter(Activity a, ArrayList<HashMap<String, String>> d) {
        activity = a;
        data=d;
        inflater = (LayoutInflater)activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        imageLoader=new ImageLoader(activity.getApplicationContext());
        apputil=new AppUtility(activity.getApplicationContext());
    }

    public int getCount() {
        return data.size();
    }

    public Object getItem(int position) {
        return position;
    }

    public long getItemId(int position) {
        return position;
    }
    
    public View getView(int position, View convertView, ViewGroup parent) {
        View vi=convertView;
        if(convertView==null)
            vi = inflater.inflate(R.layout.featured_listitem, null);

        ImageView imageview = (ImageView) vi.findViewById(R.id.img_featured_image);
        TextView tv_name=(TextView)vi.findViewById(R.id.tv_featured_itemname);
        
        try
	       {
	    	   
	    	   
	    	String tempurl=data.get(position).get("id");
	    	if(tempurl != null && !tempurl.equals(""))
	    	{
	    		imageLoader.DisplayImage(apputil.topstoreimagesurl+tempurl+".jpg", imageview);
	    		Log.e("Store image url", apputil.topstoreimagesurl+tempurl+".jpg");
	    	//imageview.setImageBitmap(getBitmapFromURL(data.get(position).get("image")));
	    	}
	    	
	        String ts1=data.get(position).get("Name");
	        Log.e("arralist_name", ts1);
	        tv_name.setText(ts1);
	        
	        

	      
	       }
	       catch(Exception e)
	       {
	    	   e.printStackTrace();
	       }
        return vi;
    }
}
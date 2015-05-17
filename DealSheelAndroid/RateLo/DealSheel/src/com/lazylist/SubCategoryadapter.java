package com.lazylist;

import java.util.ArrayList;
import java.util.HashMap;

import com.dealsheel.R;

import android.R.color;
import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

public class SubCategoryadapter extends BaseAdapter {
    
    private Activity activity;
    ArrayList<HashMap<String, String>> data;
    private static LayoutInflater inflater=null;
    public ImageLoader imageLoader; 
    
    public SubCategoryadapter(Activity a, ArrayList<HashMap<String, String>> d) {
        activity = a;
        data=d;
        inflater = (LayoutInflater)activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        imageLoader=new ImageLoader(activity.getApplicationContext());
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
            vi = inflater.inflate(R.layout.cat_item, null);

        TextView tv_name=(TextView)vi.findViewById(R.id.tv_catitem_name);
        try
	       {
	    	   
        		    	
	        String ts1=data.get(position).get("name");
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
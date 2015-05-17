package com.lazylist;

import java.util.ArrayList;
import java.util.HashMap;

import com.dealsheel.R;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

@SuppressLint("ResourceAsColor")
public class Categoryadapter extends BaseAdapter {
    
    private Activity activity;
    ArrayList<HashMap<String, String>> data;
    private static LayoutInflater inflater=null;
    public ImageLoader imageLoader; 
    String pos;
    
    public Categoryadapter(Activity a, ArrayList<HashMap<String, String>> d,String p) {
        activity = a;
        data=d;
        pos=p;
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

       // ImageView imageview = (ImageView) vi.findViewById(R.id.img_featured_image);
        TextView tv_name=(TextView)vi.findViewById(R.id.tv_catitem_name);
        LinearLayout tvt=(LinearLayout)vi.findViewById(R.id.lnv_catitem);
        ImageView img=(ImageView)vi.findViewById(R.id.img_catitem_downarrow);
        
        img.setVisibility(View.GONE);
        
        if(pos != "")
        {
        	int tm=Integer.parseInt(pos);
        	if(tm == position )
        	{
        		tvt.setBackgroundColor(activity.getResources().getColor(R.color.subcat_background));
        		img.setVisibility(View.VISIBLE);
        	}
        }
        
		
        
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
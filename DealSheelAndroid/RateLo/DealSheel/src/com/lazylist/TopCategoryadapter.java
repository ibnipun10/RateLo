package com.lazylist;

import java.util.ArrayList;
import java.util.HashMap;

import com.dealsheel.FIlterSubcategoryActivity;
import com.dealsheel.StorelistActivity;
import com.dealsheel.R;
import com.utility.AppUtility;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.View.OnClickListener;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

public class TopCategoryadapter extends BaseAdapter {
    
    private Activity activity;
    ArrayList<HashMap<String, String>> data;
    private static LayoutInflater inflater=null;
    public ImageLoader imageLoader; 
    AppUtility apputil;
    
    public TopCategoryadapter(Activity a, ArrayList<HashMap<String, String>> d) {
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
    
    public View getView(final int position, View convertView, ViewGroup parent) {
        View vi=convertView;
        if(convertView==null)
            vi = inflater.inflate(R.layout.featured_listitem, null);

        ImageView imageview = (ImageView) vi.findViewById(R.id.img_featured_image);
        TextView tv_name=(TextView)vi.findViewById(R.id.tv_featured_itemname);
        
        try
	       {
	    	   
        	 Log.i("arraylist", data+"");
	    	String tempurl=data.get(position).get("id");
	    	if(tempurl != null && !tempurl.equals(""))
	    	{
	    		imageLoader.DisplayImage(apputil.categoryimagesurl+tempurl+".jpg", imageview);
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
        
       /* vi.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				
				Intent i=new Intent(activity, FIlterSubcategoryActivity.class);
				i.putExtra("id", data.get(position).get("id"));
				i.putExtra("name", data.get(position).get("Name"));
				
				activity.startActivity(i);
			}
		});*/
        return vi;
    }
}
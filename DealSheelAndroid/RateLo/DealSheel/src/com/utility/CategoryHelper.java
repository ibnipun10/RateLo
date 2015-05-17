package com.utility;

import java.util.ArrayList;
import java.util.HashMap;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

public class CategoryHelper {
	
	ArrayList<HashMap<String, String>> contactList;
    private static final int DATABASE_VERSION = 2;
    private static final String DATABASE_NAME = "dealsheel.db";
    
    private static final String TABLE_NAME = "state";
    public static final String COLUMN_ID = "id";
    public static final String COLUMN_NAME = "Name";
    public static final String COLUMN_Ctime = "createTime";
    public static final String COLUMN_Lupdate = "lastUpdated";
    public static final String COLUMN_Lat = "Lattitude";
    public static final String COLUMN_Long = "Longitude";
    
    private static final String CAT_TABLE_NAME = "category";
    public static final String COLUMN1_ID = "id";
    public static final String COLUMN1_NAME = "Name";
    public static final String COLUMN1_Ctime = "createTime";
    public static final String COLUMN1_Lupdate = "lastUpdated";
    
    private static final String SUBALL_TABLE_NAME = "Allsubcat";
    public static final String COLUMN2_ID = "id";
    public static final String COLUMN2_NAME = "Name";
    public static final String COLUMN2_Ctime = "createTime";
    public static final String COLUMN2_Lupdate = "lastUpdated";
    public static final String COLUMN2_Views = "Views";
    public static final String COLUMN2_Subcatid = "SubCategoryID";
    
    private static final String SUBCAT_TABLE_NAME = "Subcat";
    public static final String COLUMN3_ID = "id";
    public static final String COLUMN3_NAME = "Name";
    public static final String COLUMN3_Ctime = "createTime";
    public static final String COLUMN3_Lupdate = "lastUpdated";
    public static final String COLUMN3_Views = "Views";
    public static final String COLUMN3_Subcatid = "SubCategoryID";
    public static final String COLUMN3_catid = "CategoryID";
    
    Category openHelper;
    private SQLiteDatabase database;

    public CategoryHelper(Context context){
        openHelper = new Category(context);
        database = openHelper.getWritableDatabase();
    }
    
    public void saveState(String id, String name,String ctime, String lupdate,String lat, String longi) 
    {
        ContentValues contentValues = new ContentValues();
        contentValues.put(COLUMN_ID, id);
        contentValues.put(COLUMN_NAME, name);
        contentValues.put(COLUMN_Ctime, ctime);
        contentValues.put(COLUMN_Lupdate, lupdate);
        contentValues.put(COLUMN_Lat, lat);
        contentValues.put(COLUMN_Long, longi);
        database.insert(TABLE_NAME, null, contentValues);
        }
    
    public void saveCategory(String id, String name,String ctime, String lupdate) 
    {
        ContentValues contentValues = new ContentValues();
        contentValues.put(COLUMN1_ID, id);
        contentValues.put(COLUMN1_NAME, name);
        contentValues.put(COLUMN1_Ctime, ctime);
        contentValues.put(COLUMN1_Lupdate, lupdate);
        database.insert(CAT_TABLE_NAME, null, contentValues);
        }
    
    public void saveAllsubcat(String id, String name,String ctime, String lupdate,String view, String subcat) 
    {
        ContentValues contentValues = new ContentValues();
        contentValues.put(COLUMN2_ID, id);
        contentValues.put(COLUMN2_NAME, name);
        contentValues.put(COLUMN2_Ctime, ctime);
        contentValues.put(COLUMN2_Lupdate, lupdate);
        contentValues.put(COLUMN2_Views, view);
        contentValues.put(COLUMN2_Subcatid, subcat);
        database.insert(SUBALL_TABLE_NAME, null, contentValues);
        }
    
    public void savesubcat(String id, String name,String ctime, String lupdate,String view, String subcat,String catid) 
    {
        ContentValues contentValues = new ContentValues();
        contentValues.put(COLUMN3_ID, id);
        contentValues.put(COLUMN3_NAME, name);
        contentValues.put(COLUMN3_Ctime, ctime);
        contentValues.put(COLUMN3_Lupdate, lupdate);
        contentValues.put(COLUMN3_Views, view);
        contentValues.put(COLUMN3_Subcatid, subcat);
        contentValues.put(COLUMN3_catid, catid);
        database.insert(SUBCAT_TABLE_NAME, null, contentValues);
        }
    
    
    public ArrayList<HashMap<String, String>> getstateList() 
    {
    	contactList=new ArrayList<HashMap<String,String>>();
    	
    	database=openHelper.getReadableDatabase();
         Cursor cursor = database.rawQuery("select * from " + TABLE_NAME, null);
         Log.e("no of records", cursor.getCount()+"");
         if (cursor.moveToFirst()) {
             do
             {
            	 HashMap<String, String> contact = new HashMap<String, String>();
				contact.put("id", cursor.getString(0));
				contact.put("name", cursor.getString(1));
				contact.put("ctime", cursor.getString(2));
				contact.put("lupdate", cursor.getString(3));
				contact.put("lat", cursor.getString(4));
				contact.put("long", cursor.getString(5));
				contactList.add(contact);
             } while (cursor.moveToNext());
         }
     
         
         Log.e("local db records", contactList+"");
         // return user list
         return contactList;
    }
    
    public String[] getstatesname()
    {
    	String[] str = null;
    	database=openHelper.getReadableDatabase();
        Cursor cursor = database.rawQuery("select * from " + TABLE_NAME, null);
        Log.e("no of records", cursor.getCount()+"");
        if (cursor.moveToFirst()) {
        	
        	str = new String[cursor.getCount()];
            int i = 0;
            do
            {
                 str[i] = cursor.getString(1);
                 Log.e("state name", str[i]);
                 i++;
                
             }while (cursor.moveToNext());
        }
        
        return str;
    }
    
    public ArrayList<HashMap<String, String>> getCategorylist() 
    {
    	contactList=new ArrayList<HashMap<String,String>>();
    	
    	database=openHelper.getReadableDatabase();
         Cursor cursor = database.rawQuery("select * from " + CAT_TABLE_NAME, null);
         Log.e("no of category", cursor.getCount()+"");
         if (cursor.moveToFirst()) {
             do
             {
            	 HashMap<String, String> contact = new HashMap<String, String>();
				contact.put("id", cursor.getString(0));
				contact.put("name", cursor.getString(1));
				contact.put("ctime", cursor.getString(2));
				contact.put("lupdate", cursor.getString(3));
				contactList.add(contact);
             } while (cursor.moveToNext());
         }
     
         Log.e("local db category", contactList+"");
         return contactList;
    }
    
    public ArrayList<HashMap<String, String>> getAllsubcat() 
    {
    	contactList=new ArrayList<HashMap<String,String>>();
    	
    	database=openHelper.getReadableDatabase();
         Cursor cursor = database.rawQuery("select * from " + SUBALL_TABLE_NAME, null);
         Log.e("no of records", cursor.getCount()+"");
         if (cursor.moveToFirst()) {
             do
             {
            	 HashMap<String, String> contact = new HashMap<String, String>();
				contact.put("id", cursor.getString(0));
				contact.put("name", cursor.getString(1));
				contact.put("ctime", cursor.getString(2));
				contact.put("lupdate", cursor.getString(3));
				contact.put("view", cursor.getString(4));
				contact.put("subcatid", cursor.getString(5));
				contactList.add(contact);
             } while (cursor.moveToNext());
         }
     
         
         Log.e("local db records", contactList+"");
         // return user list
         return contactList;
    }
    
    public String getmaincatname(String cid) 
    {
    	contactList=new ArrayList<HashMap<String,String>>();
    	String mainname = null;
    	database=openHelper.getReadableDatabase();
         Cursor cursor = database.rawQuery("select * from " + SUBCAT_TABLE_NAME, null);
         Log.e("subcat records", cursor.getCount()+"");
         if (cursor.moveToFirst()) {
             do
             {
            	 if(cid == cursor.getString(0))
            	 {
            	 String tempid=cursor.getString(6);
            	 Cursor cursor1 = database.rawQuery("select * from " + CAT_TABLE_NAME, null);
            	 Log.e("main records", cursor1.getCount()+"");
            	 if (cursor1.moveToFirst()) {
                     do
                     {
                    	 if(cid == cursor1.getString(0))
                    	 {
                    	 mainname=cursor1.getString(1);
                    	 Log.e("maincatname==>", mainname);
                    	 }
                     } while (cursor1.moveToNext());
            	 }
            	 
                 }
             } while (cursor.moveToNext());
         }
     
         
         Log.e("local db records", contactList+"");
         // return user list
         return mainname;
    }
    
    public ArrayList<HashMap<String, String>> getsubcat(String cid) 
    {
    	contactList=new ArrayList<HashMap<String,String>>();
    	
    	database=openHelper.getReadableDatabase();
         Cursor cursor = database.rawQuery("select * from " + SUBCAT_TABLE_NAME+" where "+COLUMN3_catid+" = '"+cid+"'", null);
         Log.e("no of records", cursor.getCount()+"");
         if (cursor.moveToFirst()) {
             do
             {
            	 HashMap<String, String> contact = new HashMap<String, String>();
				contact.put("id", cursor.getString(0));
				contact.put("name", cursor.getString(1));
				contact.put("ctime", cursor.getString(2));
				contact.put("lupdate", cursor.getString(3));
				contact.put("view", cursor.getString(4));
				contact.put("subcatid", cursor.getString(5));
				contact.put("catid", cursor.getString(6));
				contactList.add(contact);
             } while (cursor.moveToNext());
         }
     
         
         Log.e("local db records", contactList+"");
         // return user list
         return contactList;
    }
    
    public void deleteAll()
    {
    	  database.delete(TABLE_NAME, null, null);
    	  database.delete(SUBALL_TABLE_NAME, null, null);
    	  database.delete(SUBCAT_TABLE_NAME, null, null);
    	  database.delete(CAT_TABLE_NAME, null, null);
    }
    
    private class Category extends SQLiteOpenHelper 
    {

        public Category(Context context) {
            // TODO Auto-generated constructor stub
            super(context, DATABASE_NAME, null, DATABASE_VERSION);
        }

        @Override
        public void onCreate(SQLiteDatabase db) {
            // TODO Auto-generated method stub
            db.execSQL("CREATE TABLE " + TABLE_NAME + "( "
                    + COLUMN_ID + " TEXT, "
                    + COLUMN_NAME + " TEXT,"+COLUMN_Ctime+" TEXT,"+COLUMN_Lupdate+" TEXT,"+COLUMN_Lat+" TEXT,"
                    +COLUMN_Long+" TEXT)" );

            db.execSQL("CREATE TABLE " + CAT_TABLE_NAME + "( "
                    + COLUMN1_ID + " TEXT, "
                    + COLUMN1_NAME + " TEXT,"+COLUMN1_Ctime+" TEXT,"+COLUMN1_Lupdate+" TEXT)" );
            
            db.execSQL("CREATE TABLE " + SUBALL_TABLE_NAME + "( "
                    + COLUMN2_ID + " TEXT, "
                    + COLUMN2_NAME + " TEXT,"+COLUMN2_Ctime+" TEXT,"+COLUMN2_Lupdate+" TEXT,"+COLUMN2_Views+" TEXT,"
                    +COLUMN2_Subcatid+" TEXT)" );
            
            db.execSQL("CREATE TABLE " + SUBCAT_TABLE_NAME + "( "
                    + COLUMN3_ID + " TEXT, "
                    + COLUMN3_NAME + " TEXT,"+COLUMN3_Ctime+" TEXT,"+COLUMN3_Lupdate+" TEXT,"+COLUMN3_Views+" TEXT,"
                    +COLUMN3_Subcatid+" TEXT,"+COLUMN3_catid+" TEXT)" );
        }

        @Override
        public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
            // TODO Auto-generated method stub
            db.execSQL("DROP TABLE IF EXISTS"+ TABLE_NAME);
            onCreate(db);
        }

    }
}
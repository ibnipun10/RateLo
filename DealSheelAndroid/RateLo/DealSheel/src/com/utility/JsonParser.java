package com.utility;

import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.Socket;
import java.net.UnknownHostException;
import java.security.KeyManagementException;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.UnrecoverableKeyException;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.util.ArrayList;

import javax.net.ssl.SSLContext;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.HttpVersion;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.PlainSocketFactory;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.message.BasicHeader;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.apache.http.params.HttpProtocolParams;
import org.apache.http.protocol.HTTP;
import org.json.JSONException;
import org.json.JSONObject;



import android.util.Log;

public class JsonParser {
	 static InputStream is = null;
	    static JSONObject jObj = null;
	    static String json = "";
	    // constructor
	    public JsonParser() {
	 
	    }
	    public static String getJSONParam(
				ArrayList<NameValuePair> ParamsNameValuePair) {
			String jsonParam = null;
			JSONObject jsonObject = null;
			try {
				jsonObject = new JSONObject();
				for (NameValuePair element : ParamsNameValuePair) {
					Log.i("JSONPARAM", "element.getName() : " + element.getName());
					Log.i("JSONPARAM", "element.getValue() : " + element.getValue());
					if (element.getValue() == null)
						jsonObject.put(element.getName(), JSONObject.NULL);
					else if (element.getValue().indexOf("{") == 0
							&& element.getValue().lastIndexOf("}") == element
									.getValue().length() - 1) {
						/*
						 * if(AppUtils.DEBUG_MODE)
						 * Log.i(AppUtils.LOG_TAG,"JSONObject found : "
						 * +element.getValue());
						 */

						jsonObject.put(element.getName(),
								(new JSONObject(element.getValue())));
						return element.getValue();
					} else
						jsonObject.put(element.getName(), element.getValue());
					element = null;
				}

				// jsonObject.toString()
				// jsonParam = "["+jsonObject.toString()+"]";
				// square bracket removed to make it compatible with php
				// web-service.
				jsonParam = jsonObject.toString();
				
				 Log.i("JsonObject","jsonParam : "+jsonParam);
				
			} catch (Exception e) {
				// TODO: handle exception
				e.printStackTrace();
			}
			return jsonParam;
		}

		public static String doFetchDataFromWebService(String WebServiceURL,
				String WebServiceParamName,
				ArrayList<NameValuePair> ParamsNameValuePair) {
			String r = "";
			ArrayList<NameValuePair> WSParamsNameValuePair = null;
			if (WebServiceURL != null) {

				if (ParamsNameValuePair != null) {
					WSParamsNameValuePair = new ArrayList<NameValuePair>();
					WSParamsNameValuePair
							.add(new BasicNameValuePair(WebServiceParamName,
									getJSONParam(ParamsNameValuePair)));
				}
			}
			try {

				StringBuffer responseStringBuffer = new StringBuffer();
				
				String line = null;
				HttpPost WSHttpPost = null;
				HttpClient WSHttpClient = null;
				HttpResponse WSHttpResponse = null;
				BufferedReader WSBufferedReader = null;
				UrlEncodedFormEntity WSUrlEncodedFormEntity = null;
				HttpParams httpParameters = new BasicHttpParams();
JsonParser jp=new JsonParser();
				WSHttpClient =jp.getNewHttpClient();
				
				WSHttpPost = new HttpPost(WebServiceURL);
				WSUrlEncodedFormEntity = new UrlEncodedFormEntity(
						WSParamsNameValuePair);
				WSHttpPost.setEntity(WSUrlEncodedFormEntity);
				Log.i("URl",WSHttpPost.getURI().toString());
				WSHttpResponse = WSHttpClient.execute(WSHttpPost);
				InputStream is = WSHttpResponse.getEntity().getContent();
				if (is != null) {
					ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
					int len = -1;
					byte[] buff = new byte[10000];
					while ((len = is.read(buff)) != -1) {
						outputStream.write(buff, 0, len);
					}
					byte[] bs = outputStream.toByteArray();
					r = new String(bs);
					is.close();
					outputStream.flush();
					outputStream.close();
				}
				if (!r.equalsIgnoreCase("")) {
					Log.e("RESPONSE", r);
				}

			} catch (Exception e) {
				// TODO: handle exception
				e.printStackTrace();
			}
			
			
			return r;

		}

		public HttpClient getNewHttpClient() {
			try {
				KeyStore trustStore = KeyStore.getInstance(KeyStore
						.getDefaultType());
				trustStore.load(null, null);

				SSLSocketFactory sf = new MySSLSocketFactory(trustStore);
				sf.setHostnameVerifier(SSLSocketFactory.ALLOW_ALL_HOSTNAME_VERIFIER);

				HttpParams params = new BasicHttpParams();
				HttpProtocolParams.setVersion(params, HttpVersion.HTTP_1_1);
				HttpProtocolParams.setContentCharset(params, HTTP.UTF_8);

				SchemeRegistry registry = new SchemeRegistry();
				registry.register(new Scheme("http", PlainSocketFactory
						.getSocketFactory(), 80));
				registry.register(new Scheme("https", sf, 443));

				ClientConnectionManager ccm = new ThreadSafeClientConnManager(
						params, registry);
	Log.i("Keystore",ccm.toString());
				return new DefaultHttpClient(ccm, params);
			} catch (Exception e) {
				return new DefaultHttpClient();
			}
		}

		public class MySSLSocketFactory extends SSLSocketFactory {
			SSLContext sslContext = SSLContext.getInstance("TLS");

			public MySSLSocketFactory(KeyStore truststore)
					throws NoSuchAlgorithmException, KeyManagementException,
					KeyStoreException, UnrecoverableKeyException {
				super(truststore);

				TrustManager tm = new X509TrustManager() {
					public void checkClientTrusted(X509Certificate[] chain,
							String authType) throws CertificateException {
					}

					public void checkServerTrusted(X509Certificate[] chain,
							String authType) throws CertificateException {
					}

					public X509Certificate[] getAcceptedIssuers() {
						return null;
					}
				};

				sslContext.init(null, new TrustManager[] { tm }, null);
			}

			@Override
			public Socket createSocket(Socket socket, String host, int port,
					boolean autoClose) throws IOException, UnknownHostException {
				return sslContext.getSocketFactory().createSocket(socket, host,
						port, autoClose);
			}

			@Override
			public Socket createSocket() throws IOException {
				return sslContext.getSocketFactory().createSocket();
			}
		}
	   
}

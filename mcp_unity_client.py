#!/usr/bin/env python3
"""
Unity MCP Client - 通过MCP协议与Unity通信并创建圆柱体
"""

import socket
import json
import time
import sys

class UnityMcpClient:
    def __init__(self, host='localhost', port=6400):
        self.host = host
        self.port = port
        self.socket = None
        
    def connect(self):
        """连接到Unity MCP服务器"""
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.connect((self.host, self.port))
            print(f"已连接到Unity MCP服务器 {self.host}:{self.port}")
            return True
        except Exception as e:
            print(f"连接失败: {e}")
            return False
    
    def disconnect(self):
        """断开连接"""
        if self.socket:
            self.socket.close()
            self.socket = None
            print("已断开连接")
    
    def send_command(self, command):
        """发送命令到Unity"""
        if not self.socket:
            print("未连接到服务器")
            return None
            
        try:
            # 发送命令
            command_bytes = command.encode('utf-8')
            self.socket.send(command_bytes)
            
            # 接收响应
            response = self.socket.recv(8192)
            response_text = response.decode('utf-8')
            
            return response_text
        except Exception as e:
            print(f"发送命令失败: {e}")
            return None
    
    def ping(self):
        """测试连接"""
        response = self.send_command("ping")
        if response:
            print(f"Ping响应: {response}")
            return True
        return False
    
    def create_cylinder(self, name="TestCylinder", position=[0, 0, 0], rotation=[0, 0, 0], scale=[1, 1, 1]):
        """创建圆柱体"""
        command = {
            "type": "manage_gameobject",
            "params": {
                "action": "create",
                "name": name,
                "primitiveType": "Cylinder",
                "position": position,
                "rotation": rotation,
                "scale": scale
            }
        }
        
        command_json = json.dumps(command)
        print(f"发送命令: {command_json}")
        
        response = self.send_command(command_json)
        if response:
            print(f"响应: {response}")
            try:
                response_data = json.loads(response)
                return response_data
            except json.JSONDecodeError:
                print("响应不是有效的JSON格式")
                return None
        return None

def main():
    """主函数"""
    print("Unity MCP 客户端")
    print("=" * 50)
    
    # 创建客户端
    client = UnityMcpClient()
    
    # 尝试连接
    if not client.connect():
        print("无法连接到Unity MCP服务器。请确保：")
        print("1. Unity编辑器正在运行")
        print("2. Unity MCP Bridge已启动")
        print("3. 端口6400未被占用")
        return
    
    try:
        # 测试连接
        print("\n测试连接...")
        if client.ping():
            print("连接正常！")
        else:
            print("连接测试失败")
            return
        
        # 创建圆柱体
        print("\n创建圆柱体...")
        result = client.create_cylinder(
            name="MCP_Cylinder",
            position=[0, 1, 0],  # 稍微抬高一点
            rotation=[0, 0, 0],
            scale=[1, 1, 1]
        )
        
        if result:
            print("圆柱体创建成功！")
            print(f"结果: {json.dumps(result, indent=2, ensure_ascii=False)}")
        else:
            print("圆柱体创建失败")
            
    except KeyboardInterrupt:
        print("\n用户中断")
    except Exception as e:
        print(f"发生错误: {e}")
    finally:
        client.disconnect()

if __name__ == "__main__":
    main() 